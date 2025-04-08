import React from "react"
import { useState } from "react"
import { useNavigate } from "react-router-dom"
import { toast, Toaster } from "sonner"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Alert, AlertDescription } from "@/components/ui/alert"
import { AlertCircle } from 'lucide-react'
import "./loginView.css"

export default function LoginView() {
  const navigate = useNavigate()
  
  // Credenciales hardcodeadas
  const HARDCODED_USERNAME = "developer"
  const HARDCODED_PASSWORD = "123456"

  const [username, setUsername] = useState("")
  const [password, setPassword] = useState("")
  const [error, setError] = useState("")
  const [isLoading, setIsLoading] = useState(false)

  const handleSubmit = (e) => {
    e.preventDefault()
    setError("")
    setIsLoading(true)

    // Validación básica
    if (!username || !password) {
      toast.error("Error de validación", {
        description: "Por favor completa todos los campos requeridos.",
      })
      setIsLoading(false)
      return
    }

    // Validación contra credenciales hardcodeadas
    if (username !== HARDCODED_USERNAME || password !== HARDCODED_PASSWORD) {
      setTimeout(() => {
        setIsLoading(false)
        setError("Credenciales incorrectas")
        toast.error("Error de autenticación", {
          description: "El usuario o la contraseña son incorrectos.",
        })
      }, 1000)
      return
    }

    // Simulación de proceso de login exitoso
    setTimeout(() => {
      setIsLoading(false)
      toast.success("Inicio de sesión exitoso", {
        description: "Redirigiendo al dashboard...",
      })
      
      // Redirección después de 1.5 segundos (para que se vea el mensaje)
      setTimeout(() => {
        navigate("/dashboardClient")
      }, 1500)
    }, 1000)
  }

  return (
    <div className="login-container">
      <Card className="login-card">
        <CardHeader className="card-header">
          <CardTitle className="card-title">Login</CardTitle>
          <CardDescription className="card-description">
            Enter your email and password to sign in
          </CardDescription>
        </CardHeader>
        <CardContent>
          {error && (
            <Alert variant="destructive" className="error-alert">
              <AlertCircle className="alert-icon" />
              <AlertDescription>{error}</AlertDescription>
            </Alert>
          )}

          <form onSubmit={handleSubmit}>
            <div className="form-content">
              <div className="input-group">
                <Label htmlFor="username">Username</Label>
                <Input
                  id="username"
                  type="text"
                  placeholder={HARDCODED_USERNAME}
                  value={username}
                  onChange={(e) => setUsername(e.target.value)}
                />
              </div>
              <div className="input-group">
                <div className="password-header">
                  <Label htmlFor="password">Password</Label>
                  <a href="#" className="forgot-password">
                  Forgot password?
                  </a>
                </div>
                <Input 
                  id="password" 
                  type="password" 
                  placeholder={HARDCODED_PASSWORD}
                  value={password} 
                  onChange={(e) => setPassword(e.target.value)} 
                />
              </div>

              <Button type="submit" className="submit-button" disabled={isLoading}>
                {isLoading ? "Sign in..." : "Sign in"}
              </Button>
            </div>
          </form>
        </CardContent>
        <CardFooter className="card-footer">
          <div className="signup-text">
            Don't have an account?{" "}
            <a href="#" className="signup-link">
              Sign up
            </a>
          </div>
        </CardFooter>
      </Card>
      <Toaster position="top-center" />
    </div>
  )
}