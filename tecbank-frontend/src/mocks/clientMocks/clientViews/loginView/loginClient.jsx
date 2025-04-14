"use client"

import { useState, useEffect } from "react" // Importa useEffect
import { useNavigate } from "react-router-dom"
import { toast, Toaster } from "sonner"
import { AlertCircle } from "lucide-react"
import "./loginStyle.css"

function LoginClient() {
  const [username, setUsername] = useState("")
  const [password, setPassword] = useState("")
  const [error, setError] = useState("")
  const [isLoading, setIsLoading] = useState(false)
  const [isSuccess, setIsSuccess] = useState(false)

  // Credenciales hardcodeadas para prueba
  const hardcodedCredentials = {
    username: "admin",
    password: "123456"
  }

  const navigate = useNavigate()

  // Efecto para redirección después de login exitoso
  useEffect(() => {
    if (isSuccess) {
      // Redirige después de 1.5 segundos (para que el usuario vea el mensaje)
      const redirectTimer = setTimeout(() => {
        navigate("/client_dashboard")
      }, 1500)

      // Limpieza del timer si el componente se desmonta
      return () => clearTimeout(redirectTimer)
    }
  }, [isSuccess, navigate])

  const handleSubmit = (e) => {
    e.preventDefault()
    setError("")
    setIsLoading(true)

    // Validación básica
    if (!username || !password) {
      toast.error("Error de Validación", {
        description: "Por favor complete todos los campos requeridos para continuar.",
      })
      setIsLoading(false)
      return
    }

    // Simular proceso de inicio de sesión
    setTimeout(() => {
      console.log("Intento de inicio de sesión con:", { username, password })

      // Verificar contra credenciales hardcodeadas
      if (username === hardcodedCredentials.username && password === hardcodedCredentials.password) {
        setIsSuccess(true)
        toast.success("Iniciando sesión")
      } else {
        toast.error("Credenciales Incorrectas", {
          description: "El nombre de usuario o contraseña son incorrectos.",
        })
      }

      setIsLoading(false)

      // Reiniciar formulario solo si fue exitoso
      if (isSuccess) {
        setUsername("")
        setPassword("")
      }
    }, 1000)
  }

  return (
    <div className="login-container">
      <div className="login-card">
        <div className="login-header">
          <h1 className="login-title">Iniciar Sesión</h1>
          <p className="login-description">Ingrese su nombre de usuario y contraseña para iniciar sesión</p>
          {/* Mostrar credenciales de prueba (opcional) */}
          <p className="text-sm text-gray-500 mt-2">
            Usuario de prueba: <strong>admin</strong> | Contraseña: <strong>123456</strong>
          </p>
        </div>
        <div className="login-content">
          {error && (
            <div className="error-alert">
              <AlertCircle className="error-icon" />
              <p className="error-message">{error}</p>
            </div>
          )}

          <form onSubmit={handleSubmit} className="login-form">
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