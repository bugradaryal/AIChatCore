import axios from 'axios';

//console.log('BASE URL:', import.meta.env.VITE_API_BASE_URL);   // geçici, test için

const apiClient = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  timeout: 15000,
  headers: {
    'Content-Type': 'application/json',
    'X-API-KEY': import.meta.env.VITE_API_KEY,
  },
});

export default apiClient;