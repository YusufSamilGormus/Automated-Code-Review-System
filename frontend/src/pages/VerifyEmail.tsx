import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useSearchParams, useNavigate } from 'react-router-dom';

const VerifyEmail: React.FC = () => {
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const [message, setMessage] = useState<string>('Verifying email...');
  const [isSuccess, setIsSuccess] = useState<boolean | null>(null);

  useEffect(() => {
    const token = searchParams.get('token');

    if (!token) {
      setMessage('No token provided.');
      setIsSuccess(false);
      return;
    }

    const verifyEmail = async () => {
      try {
        const response = await axios.post(
          `${process.env.REACT_APP_API_URL}/api/auth/verify-email`,
          token,
          {
            headers: {
              'Content-Type': 'application/json'
            }
          }
        );

        setMessage(response.data);
        setIsSuccess(true);

        setTimeout(() => navigate('/login'), 4000);
      } catch (error: any) {
        setMessage(error.response?.data || 'Verification failed.');
        setIsSuccess(false);
      }
    };

    verifyEmail();
  }, [searchParams, navigate]);

  return (
    <div style={{ maxWidth: '600px', margin: 'auto', paddingTop: '60px', textAlign: 'center' }}>
      <h2>Email Verification</h2>
      <p style={{ color: isSuccess === true ? 'green' : 'red', marginTop: '20px' }}>{message}</p>
      {isSuccess && <p>You’ll be redirected to login shortly...</p>}
    </div>
  );
};

export default VerifyEmail;
