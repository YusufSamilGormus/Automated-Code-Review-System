import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { Button, Container, Card, ListGroup } from 'react-bootstrap';

interface Submission {
    id: string;
    code: string;
    language: string;
    status: string;
    createdAt: string;
}

interface DashboardProps {
    // Add any props if needed
}

const Dashboard: React.FC<DashboardProps> = () => {
    const navigate = useNavigate();
    const [submissions, setSubmissions] = useState<Submission[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (!token) {
            navigate('/');
            return;
        }

        try {
            // Get user's submissions
            axios.get<Submission[]>('http://localhost:5000/api/codesubmission', {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            })
            .then(response => {
                setSubmissions(response.data);
                setLoading(false);
            })
            .catch(error => {
                console.error('Error fetching submissions:', error);
                setError('Failed to fetch submissions. Please try again later.');
                setLoading(false);
            });
        } catch (err) {
            console.error('Unexpected error:', err);
            setError('An unexpected error occurred. Please try again later.');
            setLoading(false);
        }
    }, [navigate]);

    const handleLogout = () => {
        localStorage.removeItem('token');
        localStorage.removeItem('refreshToken');
        localStorage.removeItem('expiresAt');
        navigate('/');
    };

    return (
        <Container className="mt-4">
            <div className="d-flex justify-content-between align-items-center mb-4">
                <h2>Code Submissions</h2>
                <Button variant="primary" onClick={() => navigate('/submit')}>New Submission</Button>
            </div>

            {error && (
                <div className="alert alert-danger" role="alert">
                    {error}
                </div>
            )}

            {loading ? (
                <div className="text-center">
                    <div className="spinner-border" role="status">
                        <span className="visually-hidden">Loading...</span>
                    </div>
                </div>
            ) : (
                <Card>
                    <Card.Body>
                        <ListGroup>
                            {submissions.length === 0 ? (
                                <p className="text-muted text-center">No submissions yet. Submit your first code!</p>
                            ) : (
                                submissions.map((submission) => (
                                    <ListGroup.Item key={submission.id}>
                                        <div className="d-flex justify-content-between align-items-center">
                                            <div>
                                                <h6 className="mb-1">{submission.language}</h6>
                                                <small className="text-muted">Status: {submission.status}</small>
                                            </div>
                                            <small className="text-muted">{new Date(submission.createdAt).toLocaleDateString()}</small>
                                        </div>
                                    </ListGroup.Item>
                                ))
                            )}
                        </ListGroup>
                    </Card.Body>
                </Card>
            )}

            <div className="mt-4">
                <Button variant="danger" onClick={handleLogout}>Logout</Button>
            </div>
        </Container>
    );
};

export default Dashboard;
