import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './login_admin.css'; // Archivo CSS para estilos

const LoginAdmin = () => {
    const [credentials, setCredentials] = useState({username: '',password: ''});
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();

    const handleChange = (e) => {
        const { name, value } = e.target;
        setCredentials(prev => ({...prev, [name]: value}));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setLoading(true);

        // Validaciones básicas
        if (!credentials.username || !credentials.password) {
            setError('Por favor ingrese usuario y contraseña');
            setLoading(false);
            return;
        }

        try {
            // Aquí iría la llamada a tu API de autenticación
            // Ejemplo simulado:
            const response = await fakeAuthApi(credentials);
            
            if (response.success) {
                // Guardar token y redirigir
                localStorage.setItem('tecbank_admin_token', response.token);
                navigate('/adminDashboard');
            } else {
                setError('Credenciales inválidas');
            }
        } catch (err) {
            setError('Error al conectar con el servidor');
        } finally {
            setLoading(false);
        }
    };

    // Función simulada de autenticación (reemplazar con llamada real a tu backend)
    const fakeAuthApi = async ({ username, password }) => {
        return new Promise(resolve => {
            setTimeout(() => {
                if (username === 'admin' && password === 'TecBank2023') {
                    resolve({ success: true, token: 'fake-jwt-token' });
                } 
                else {
                    resolve({ success: false });
                }
            }, 1000);
        });
    };

    return (
        <div className="login-admin-container">
            <div className="login-admin-card">
                <div className="login-header">
                <img 
                    src="/tecbank-logo.png" 
                    alt="TecBank Logo" 
                    className="logo"
                />
                <h2>Administración TecBank</h2>
                <p>Ingrese sus credenciales para acceder</p>
                </div>

                <form onSubmit={handleSubmit} className="login-form">
                    {error && <div className="alert alert-danger">{error}</div>}
                    
                    <div className="form-group">
                        <label htmlFor="username">Usuario</label>
                        <input
                        type="text"
                        id="username"
                        name="username"
                        value={credentials.username}
                        onChange={handleChange}
                        placeholder="Ingrese su usuario"
                        className="form-control"
                        />
                    </div>

                    <div className="form-group">
                        <label htmlFor="password">Contraseña</label>
                        <input
                        type="password"
                        id="password"
                        name="password"
                        value={credentials.password}
                        onChange={handleChange}
                        placeholder="Ingrese su contraseña"
                        className="form-control"
                        />
                    </div>

                    <button 
                        type="submit" 
                        className="btn-login"
                        disabled={loading}
                    >
                        {loading ? 'Cargando...' : 'Iniciar Sesión'}
                    </button>
                </form>

                <div className="login-footer">
                </div>
            </div>
        </div>
    );
};

export default LoginAdmin;