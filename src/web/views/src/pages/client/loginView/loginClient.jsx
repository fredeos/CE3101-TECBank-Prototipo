"use client"

import { useState, useEffect } from "react"
import { useNavigate } from "react-router-dom"
import { toast, Toaster } from "sonner"
import { AlertCircle } from "lucide-react"
import axios from "axios"
import "./loginStyle.css"
import { useAuth } from "@/context/AuthContext" 

function LoginClient() {

  // Estados del formulario
  const [username, setUsername] = useState("")
  const [password, setPassword] = useState("")
  const [error, setError] = useState("")
  const [isLoading, setIsLoading] = useState(false)
  const [isSuccess, setIsSuccess] = useState(false)

  // URL base del backend
  const BACKEND_URL = "http://192.168.100.59:5055"
  const navigate = useNavigate()
  const { login, user } = useAuth() // Usa el contexto

  // Efecto para redirección después de login exitoso
  useEffect(() => 
  {
    if (isSuccess && user) 
    {
      const redirectTimer = setTimeout(() => 
      {
        navigate("/client_dashboard")
      }, 1500)
      return () => clearTimeout(redirectTimer)
    }
  }, [isSuccess, navigate, user])

  const handleSubmit = async (e) => 
  {
    e.preventDefault()
    setError("")
    setIsLoading(true)

    // Validación básica de campos vacios
    if (!username || !password) 
    {
      toast.error("Error de Validación", { description: "Por favor complete todos los campos requeridos para continuar.", })
      setIsLoading(false)
      return
    }

    try 
    {
      // Construye la URL con parámetros de consulta
      const loginUrl = `${BACKEND_URL}/services/client/login?user=${encodeURIComponent(username)}&pass=${encodeURIComponent(password)}`


      // Request al backend
      const response = await axios.get(loginUrl)

      // Verifica si la respuesta es exitosa (200 OK)
      if (response.status === 200) 
      {
        
        // Si la respuesta es string, la convierte a JSON; si ya es objeto, la usa directamente
        const responseData = typeof response.data === 'string'
          ? JSON.parse(response.data)
          : response.data

        setIsSuccess(true)
        login(responseData)

        toast.success("Inicio de sesión exitoso")
      }
    } 
    catch (error) 
    {
      console.error("Error en login:", error)

      let errorMessage = "Error al iniciar sesión"  // Mensaje generico por defecto
      
      // Manejo de errores específicos según la respuesta del backend
      if (error.response) 
      {
        if (error.response.status === 404) 
        {
          errorMessage = "Credenciales incorrectas"
        }
      } 
      else if (error.request) 
      {
        errorMessage = "No se recibió respuesta del servidor" // Si no hubo respuesta del servidor
      }

      toast.error("Error de Autenticación", {description: errorMessage,})
    } 
    finally 
    {
      setTimeout(() => 
      {

        setIsLoading(false)

        // Reiniciar formulario solo si fue exitoso
        if (isSuccess) 
        {
          setUsername("")
          setPassword("")
        }
      }, 1000)
    }
  }

  return (
    <div className="login-container">
      {/* Encabezado */}
      <div className="login-card">
        <div className="login-header-client ">
          <h1 className="login-title">Iniciar Sesión</h1>
          <p className="login-description">Ingrese su nombre de usuario y contraseña para iniciar sesión</p>
        </div>

        <div className="login-content">
          {error && (
            <div className="error-alert">
              <AlertCircle className="error-icon" />
              <p className="error-message">{error}</p>
            </div>
          )}

          {/* Formulario de inicio de sesion */}
          <form onSubmit={handleSubmit} className="login-form">

            {/* Username */}
            <div className="form-group-client">
              <label htmlFor="username" className="form-label">
                Nombre de Usuario
              </label>
              <input
                id="username"
                type="text"
                placeholder="Ingrese su nombre de usuario"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                className="form-input"
              />
            </div>
            
            {/* Password */}
            <div className="form-group-client">
              <div className="form-row">
                <label htmlFor="password" className="form-label">
                  Contraseña
                </label>
                <a href="/forgot-password" className="forgot-password">
                  ¿Olvidó su contraseña?
                </a>
              </div>
              <input
                id="password"
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className="form-input"
              />
            </div>

            <button type="submit" className="login-button" disabled={isLoading}>
              {isLoading ? "Iniciando sesión..." : "Iniciar Sesión"}
            </button>

          </form>

        </div>
      </div>
      <Toaster position="bottom-right" />

    </div>
  )
}

export default LoginClient