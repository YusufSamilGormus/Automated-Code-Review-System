import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

interface CodeReview {
  id: number;
  rating: string;
  strengths: string;
  suggestions: string;
  bestPractices: string;
  syntaxErrors: string;
  logicErrors: string;
  performanceIssues: string;
  securityIssues: string;
  codeSmells: string;
  documentationIssues: string;
  designWeaknesses: string;
  implementationWeaknesses: string;
  testingWeaknesses: string;
  maintainabilityIssues: string;
  reviewedAt: string;
  codeSubmissionId: number;
}


interface Submission {
  id: number;
  title: string;
  submittedAt: string;
  reviews: CodeReview[];
}

const ReviewHistory: React.FC = () => {
  const [submissions, setSubmissions] = useState<Submission[]>([]);
  const [error, setError] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    const fetchHistory = async () => {
      try {
        const token = localStorage.getItem('accessToken');
        const response = await axios.get(`${process.env.REACT_APP_API_URL}/api/codereview/submissions`, {
          headers: {
            Authorization: `Bearer ${token}`
          }
        });
        setSubmissions(response.data);
      } catch (err) {
        setError('Failed to load review history.');
      }
    };

    fetchHistory();
  }, []);

  return (
    <div style={{ maxWidth: '800px', margin: 'auto', paddingTop: '60px' }}>
      <h2>Review History</h2>

      {error && <p style={{ color: 'red' }}>{error}</p>}

      {submissions.length === 0 ? (
        <p>No submissions found.</p>
      ) : (
        <ul style={{ listStyle: 'none', padding: 0 }}>
          {submissions.map((sub) => (
            <li key={sub.id} style={{ border: '1px solid #ccc', padding: '15px', marginBottom: '10px' }}>
              <strong>{sub.title || 'Untitled Submission'}</strong>
              <br />
              📅 {new Date(sub.submittedAt).toLocaleString()}
              <br />
              🧪 Reviews: {sub.reviews.length}
              <br />
              <button
                onClick={() => navigate(`/review/${sub.id}`)}
                style={{ marginTop: '10px', padding: '6px 12px' }}
              >
                View
              </button>
            </li>
          ))}
        </ul>
      )}

      <button
        onClick={() => navigate('/dashboard')}
        style={{ marginTop: '30px', padding: '8px 16px' }}
      >
        Back to Dashboard
      </button>
    </div>
  );
};

export default ReviewHistory;
    