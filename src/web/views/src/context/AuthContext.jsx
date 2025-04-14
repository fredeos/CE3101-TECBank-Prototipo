import { createContext, useContext, useState } from 'react'

// Crea un nuevo contexto llamado AuthContext
const AuthContext = createContext()

// Componente proveedor del contexto, que envolverá a los componentes que necesiten acceso a la autenticación
export function AuthProvider({ children }) {
    
    const [user, setUser] = useState(() => 
    {
        // Al iniciar, intenta obtener los datos del usuario desde sessionStorage
        const storedUser = sessionStorage.getItem('userData')
        return storedUser ? JSON.parse(storedUser) : null // Si existen datos guardados, los convierte de JSON a objeto y los retorna como valor inicial
    })

    // Guarda el usuario tanto en el estado como en sessionStorage
    const login = (userData) => 
    {
        setUser(userData)
        sessionStorage.setItem('userData', JSON.stringify(userData))
    }

    // Limpia el estado y borra el dato guardado en sessionStorage
    const logout = () => 
    {
        setUser("")
        sessionStorage.removeItem('userData')
    }

    // Retorna el proveedor de contexto, pasando los valores necesarios (usuario y funciones) como valor del contexto
    return (
        <AuthContext.Provider value={{ user, login, logout }}>
            {children}
        </AuthContext.Provider>
    )
}

export const useAuth = () => useContext(AuthContext)