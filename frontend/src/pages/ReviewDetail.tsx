import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axios from 'axios';

interface ReviewDetailData {
    id: number;
    code: string;
    language: string;
    title: string;
    description: string;
    submittedAt: string;
    reviews: any[];
}

const ReviewDetail: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const [data, setData] = useState<ReviewDetailData | null>(null);
    const [error, setError] = useState('');
    const [question, setQuestion] = useState('');
    const [response, setResponse] = useState('');
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchData = async () => {
            try {
                const token = localStorage.getItem('accessToken');
                const response = await axios.get(`${process.env.REACT_APP_API_URL}/api/codereview/submission/${id}`, {
                    headers: { Authorization: `Bearer ${token}` }
                });
                setData(response.data);
            } catch (err) {
                setError('Review details could not be loaded.');
            }
        };

        fetchData();
    }, [id]);

    const handleChatSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!question.trim()) return;
        setLoading(true);
        setResponse('');

        try {
            const token = localStorage.getItem('accessToken');
            const res = await axios.post(
                `${process.env.REACT_APP_API_URL}/api/codereview/continue-review`,
                {
                    submissionId: Number(id),
                    question
                },
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                        'Content-Type': 'application/json'
                    }
                }
            );
            setResponse(res.data.answer);
        } catch (err) {
            setResponse('❌ Error during follow-up chat.');
        } finally {
            setLoading(false);
        }
    };

    if (error) return <p style={{ color: 'red' }}>{error}</p>;
    if (!data) return <p>Loading...</p>;

    return (
        <div style={{ maxWidth: '900px', margin: 'auto', paddingTop: '40px' }}>
            <button onClick={() => navigate('/history')} style={{ marginBottom: '20px' }}>
                ⬅ Back to History
            </button>

            <h2>{data.title || 'Untitled Review'}</h2>
            <p><strong>Language:</strong> {data.language}</p>
            <p><strong>Description:</strong> {data.description}</p>
            <p><strong>Submitted At:</strong> {new Date(data.submittedAt).toLocaleString()}</p>

            <h3>Code:</h3>
            <pre style={{ background: '#f0f0f0', padding: '10px', whiteSpace: 'pre-wrap' }}>{data.code}</pre>

            <h3>Reviews:</h3>
            {data.reviews.length === 0 ? (
                <p>No reviews found for this submission.</p>
            ) : (
                data.reviews.map((review, index) => (
                    <div key={index} style={{ border: '1px solid #ccc', padding: '10px', marginTop: '15px' }}>
                        <p><strong>🔍 Rating:</strong> {review.rating}</p>
                        <p><strong>💡 Suggestions:</strong> {review.suggestions}</p>
                        <p><strong>✅ Strengths:</strong> {review.strengths}</p>
                        <p><strong>📘 Best Practices:</strong> {review.bestPractices}</p>
                        <p><strong>⚠️ Syntax Errors:</strong> {review.syntaxErrors}</p>
                        <p><strong>🧠 Logic Errors:</strong> {review.logicErrors}</p>
                        <p><strong>🛡️ Security Issues:</strong> {review.securityIssues}</p>
                        <p><strong>🧼 Code Smells:</strong> {review.codeSmells}</p>
                        <p><strong>📄 Documentation Issues:</strong> {review.documentationIssues}</p>
                        <p><strong>🧱 Design Weaknesses:</strong> {review.designWeaknesses}</p>
                        <p><strong>🔧 Implementation Weaknesses:</strong> {review.implementationWeaknesses}</p>
                        <p><strong>🧪 Testing Weaknesses:</strong> {review.testingWeaknesses}</p>
                        <p><strong>🔁 Maintainability Issues:</strong> {review.maintainabilityIssues}</p>
                    </div>
                ))
            )}

            <h3 style={{ marginTop: '40px' }}>Ask a follow-up question 💬</h3>
            <form onSubmit={handleChatSubmit} style={{ marginBottom: '20px' }}>
                <textarea
                    value={question}
                    onChange={(e) => setQuestion(e.target.value)}
                    placeholder="Ask something about this review..."
                    rows={4}
                    style={{ width: '100%', padding: '10px', fontFamily: 'inherit' }}
                />
                <button type="submit" disabled={loading} style={{ marginTop: '10px' }}>
                    {loading ? 'Thinking...' : 'Send Question'}
                </button>
            </form>

            {response && (
                <div style={{ backgroundColor: '#e8f4ff', padding: '10px', borderRadius: '5px' }}>
                    <strong>LLM Response:</strong>
                    <p style={{ whiteSpace: 'pre-wrap' }}>{response}</p>
                </div>
            )}
        </div>
    );
};

export default ReviewDetail;
