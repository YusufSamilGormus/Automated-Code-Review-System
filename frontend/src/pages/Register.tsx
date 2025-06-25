import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

const Register: React.FC = () => {
    const navigate = useNavigate();

    const [username, setUsername] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const handleRegister = async (e: React.FormEvent) => {
        e.preventDefault();
        setError('');
        setSuccess('');

        if (password !== confirmPassword) {
            setError("Passwords do not match.");
            return;
        }

        try {
            const response = await axios.post(`${process.env.REACT_APP_API_URL}/api/auth/register`, {
                username,
                email,
                password,
                confirmPassword
            });

            setSuccess(response.data.message || 'Registration successful! Please check your email to verify your account.');
            setTimeout(() => navigate('/login'), 2000);
        } catch (err: any) {
            if (err.response?.data) {
                setError(err.response.data);
            } else {
                setError('Registration failed. Please try again.');
            }
        }
    };

    return (
        <div style={{ maxWidth: '400px', margin: 'auto', paddingTop: '100px' }}>
            <h2>Register</h2>
            <form onSubmit={handleRegister}>
                <div>
                    <label>Username:</label>
                    <input
                        type="text"
                        value={username}
                        required
                        onChange={(e) => setUsername(e.target.value)}
                        style={{ width: '100%', padding: '8px' }}
                    />
                </div>
                <div style={{ marginTop: '10px' }}>
                    <label>Email:</label>
                    <input
                        type="email"
                        value={email}
                        required
                        onChange={(e) => setEmail(e.target.value)}
                        style={{ width: '100%', padding: '8px' }}
                    />
                </div>
                <div style={{ marginTop: '10px' }}>
                    <label>Password:</label>
                    <input
                        type="password"
                        value={password}
                        required
                        onChange={(e) => setPassword(e.target.value)}
                        style={{ width: '100%', padding: '8px' }}
                    />
                </div>
                <div style={{ marginTop: '10px' }}>
                    <label>Confirm Password:</label>
                    <input
                        type="password"
                        value={confirmPassword}
                        required
                        onChange={(e) => setConfirmPassword(e.target.value)}
                        style={{ width: '100%', padding: '8px' }}
                    />
                </div>

                {error && <p style={{ color: 'red', marginTop: '10px' }}>{error}</p>}
                {success && <p style={{ color: 'green', marginTop: '10px' }}>{success}</p>}

                <button type="submit" style={{ marginTop: '20px', width: '100%' }}>
                    Register
                </button>
            </form>
        </div>
    );
};

export default Register;
