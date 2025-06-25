import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

const Dashboard: React.FC = () => {
    const navigate = useNavigate();

    useEffect(() => {
        const token = localStorage.getItem('accessToken');
        if (!token) {
            navigate('/login');
        }
    }, []);

    const [code, setCode] = useState('');
    const [review, setReview] = useState('');
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setReview('');
        setError('');

        try {
            const token = localStorage.getItem('accessToken');
            const response = await axios.post(
                `${process.env.REACT_APP_API_URL}/api/codereview/submit`,
                { code },
                {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                }
            );

            const reviewData = response.data.reviews[0];

            const reviewSummary = `
🟢 Strengths:
${reviewData.strengths}

🟡 Suggestions:
${reviewData.suggestions}

🔵 Best Practices:
${reviewData.bestPractices}

🛠 Syntax Errors:
${reviewData.syntaxErrors}

🧠 Logic Errors:
${reviewData.logicErrors}

🚨 Security Issues:
${reviewData.securityIssues}

🧼 Code Smells:
${reviewData.codeSmells}

🧾 Documentation Issues:
${reviewData.documentationIssues}

🧱 Design Weaknesses:
${reviewData.designWeaknesses}

🔧 Implementation Weaknesses:
${reviewData.implementationWeaknesses}

🧪 Testing Weaknesses:
${reviewData.testingWeaknesses}

🔁 Maintainability Issues:
${reviewData.maintainabilityIssues}
`;

            setReview(reviewSummary);
        } catch (err: any) {
            setError('Code review failed. Please try again.');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div style={{ maxWidth: '800px', margin: 'auto', paddingTop: '60px' }}>
            {/* Navigation Buttons */}
            <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '20px' }}>
                <button
                    onClick={() => navigate('/history')}
                    style={{
                        backgroundColor: '#eee',
                        border: '1px solid #ccc',
                        padding: '8px 16px',
                        cursor: 'pointer',
                        borderRadius: '4px'
                    }}
                >
                    Review History
                </button>

                <button
                    onClick={() => {
                        localStorage.removeItem('accessToken');
                        navigate('/login');
                    }}
                    style={{
                        backgroundColor: '#eee',
                        border: '1px solid #ccc',
                        padding: '8px 16px',
                        cursor: 'pointer',
                        borderRadius: '4px'
                    }}
                >
                    Logout
                </button>
            </div>

            <h2>Code Review Panel</h2>
            <form onSubmit={handleSubmit}>
                <textarea
                    value={code}
                    onChange={(e) => setCode(e.target.value)}
                    placeholder="Paste your code here..."
                    rows={10}
                    style={{ width: '100%', fontFamily: 'monospace', padding: '10px' }}
                    required
                />
                <button type="submit" style={{ marginTop: '15px' }} disabled={loading}>
                    {loading ? 'Reviewing...' : 'Submit for Review'}
                </button>
            </form>

            {error && <p style={{ color: 'red', marginTop: '10px' }}>{error}</p>}
            {review && (
                <div style={{ marginTop: '30px' }}>
                    <h4>Review Result:</h4>
                    <pre style={{ background: '#f3f3f3', padding: '15px', whiteSpace: 'pre-wrap' }}>
                        {review}
                    </pre>
                </div>
            )}
        </div>
    );
};

export default Dashboard;
