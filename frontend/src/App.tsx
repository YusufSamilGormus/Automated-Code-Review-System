import React from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import Login from './pages/Login';
import Register from './pages/Register';
import Dashboard from './pages/Dashboard';
import ReviewHistory from './pages/ReviewHistory';
import ReviewDetail from './pages/ReviewDetail';
import VerifyEmail from './pages/VerifyEmail';

const App: React.FC = () => {
  return (
    <Routes>
      <Route path="/" element={<Navigate to="/login" />} />
      <Route path="/login" element={<Login />} />
      <Route path="/register" element={<Register />} />
      <Route path="/dashboard" element={<Dashboard />} />
      <Route path="/history" element={<ReviewHistory />} />
      <Route path="/review/:id" element={<ReviewDetail />} />
      <Route path="/verify-email" element={<VerifyEmail />} />
    </Routes>
  );
};

export default App;
