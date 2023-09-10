import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import { ApiContextProvider } from "./contexts/ApiContext";

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <ApiContextProvider>
    <App />
  </ApiContextProvider>
);

