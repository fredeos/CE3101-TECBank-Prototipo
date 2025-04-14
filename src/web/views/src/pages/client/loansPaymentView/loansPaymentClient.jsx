"use client"

import { useState, useEffect } from "react"
import { useNavigate } from "react-router-dom"
import { toast, Toaster } from "sonner"
import { ArrowLeft, Info, PiggyBank } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { RadioGroup, RadioGroupItem } from "@/components/ui/radio-group"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Badge } from "@/components/ui/badge"
import { useAuth } from "@/context/AuthContext"
import "./loansPaymentStyle.css"

function LoanPayments() {
    
    const navigate = useNavigate()
    const { user } = useAuth() // Obtenemos los datos del usuario
    const currentDate = new Date().toLocaleDateString()

    // Estados para datos
    const [loans, setLoans] = useState([])
    const [accounts, setAccounts] = useState([])
    const [loading, setLoading] = useState(true)
    const [error, setError] = useState(null)

    // Estados del formulario
    const [selectedLoanId, setSelectedLoanId] = useState("")
    const [paymentType, setPaymentType] = useState("regular")
    const [paymentAmount, setPaymentAmount] = useState("")
    const [paymentAccountId, setPaymentAccountId] = useState("")
    const [isConfirmStep, setIsConfirmStep] = useState(false)
    const [isLoadingSubmit, setIsLoadingSubmit] = useState(false)
    const [isLoading, setIsLoading] = useState(false)

    // Obtener datos del backend
    useEffect(() => {
        
        if (!user?.id) // Si no hay usuario o no tiene un ID, no hace nada
            return

        const fetchData = async () => 
        {
            try 
            {
                setLoading(true)

                // Obtenemos los prestamos
                const loansResponse = await fetch(
                    `http://192.168.100.59:5055/services/client/${user.id}/loans`
                )
                const loansData = await loansResponse.json()
                setLoans(loansData) // Guarda los prestamos en el estado

                // Obtener cuentas
                const accountsResponse = await fetch(
                    `http://192.168.100.59:5055/services/client/${user.id}/accounts`
                )
                const accountsData = await accountsResponse.json()
                setAccounts(accountsData) // Guarda las cuentas en el estado

            } catch (err) {
                console.error("Error fetching data:", err)
                setError("No se pudieron cargar los datos")
                toast.error("Error", {
                    description: "No se pudieron cargar los datos de préstamos y cuentas",
                })
            } finally {
                setLoading(false)
            }
        }

        fetchData()
    }, [user?.id]) // Se ejecuta cada vez que cambia el ID del usuario


    const selectedLoan = loans.find((loan) => loan.id.toString() === selectedLoanId) // Busca el préstamo seleccionado por su ID
    const selectedAccount = accounts.find((account) => account.id === paymentAccountId) // Busca la cuenta seleccionada por su ID

    // Calcular pago mensual estimado
    const calculateMonthlyPayment = (loan) => 
    {
        if (!loan) 
            return 0 // Si no hay préstamo, retorno 0
        
        // Fermula: cuota mensual fija (interés compuesto)
        const monthlyRate = loan.interest_rate / 100 / 12
        const payment = (loan.balance * monthlyRate) / (1 - Math.pow(1 + monthlyRate, -loan.lapse))
        return payment
    }

    // Manejar selección de préstamo
    const handleLoanSelect = (loanId) => 
    {
        const loan = loans.find((loan) => loan.id.toString() === loanId) // Encuentra el prestamo
        setSelectedLoanId(loanId) // Guarda el ID seleccionado
        setPaymentAmount(calculateMonthlyPayment(loan).toFixed(2)) // Calcula y guarda el pago mensual
        setPaymentType("regular")
        setIsConfirmStep(false)
    }

    // Manejar cambio de tipo de pago
    const handlePaymentTypeChange = (value) => 
    {
        setPaymentType(value)
        if (value === "regular" && selectedLoan) 
        {
            setPaymentAmount(calculateMonthlyPayment(selectedLoan).toFixed(2)) // Si es un pago regular, se recalcula el monto
        } 
        else if (value === "extraordinary") 
        {
            setPaymentAmount("") // Si es extraordinario, deja el monto vacío para entrada manual
        }
    }

    // Manejo de cambio de moneda
    const getCurrencySymbol = (currencyId) => 
    {
        const currencySymbols = 
        {
            1: '$', // Dólar
            2: '€', // Euro
            3: '₡'  // Colón costarricense
        };
        return currencySymbols[currencyId] || '$';
    };

    // Manejar envío del formulario
    const handleSubmit = async (e) => {
        e.preventDefault()

        // Validar formulario no vacio
        if (!selectedLoanId || !paymentAmount || !paymentAccountId) 
        {
            toast.error("Información Faltante", {description: "Por favor complete todos los campos requeridos para continuar.",})
            return
        }

        // Validacion de valor en formulario valido
        if (isNaN(paymentAmount) || Number.parseFloat(paymentAmount) <= 0) 
        {
            toast.error("Monto Inválido", {description: "Por favor ingrese un monto de pago válido.",})
            return
        }

        // Validacion de si el saldo de cuenta insuficiente
        if (selectedAccount && Number.parseFloat(paymentAmount) > selectedAccount.balance) {
            toast.error("Fondos Insuficientes", {description: "El saldo de su cuenta es insuficiente para este pago.",})
            return
        }

        // Validacion de si el monto a pagar no excede el pendiente
        if (selectedLoan && Number.parseFloat(paymentAmount) > selectedLoan.balance) 
        {
            toast.error("Monto Excede Saldo", { description: "El monto del pago no puede exceder el saldo pendiente del préstamo.",})
            return
        }

        // Validacion de paso de confirmacion
        if (!isConfirmStep) 
        {
            setIsConfirmStep(true)
            return
        }

        setIsLoadingSubmit(true)

        try {
            // Aquí iría la llamada a la API para procesar el pago
            // const response = await fetch(...)

            // Simulación de éxito
            toast.success("Pago Exitoso", { description: `El pago de ${getCurrencySymbol(selectedAccount.currency_id)}${paymentAmount} ha sido procesado exitosamente.`,})

            // Reiniciar formulario
            setIsConfirmStep(false)
            setSelectedLoanId("")
            setPaymentType("regular")
            setPaymentAmount("")
            setPaymentAccountId("")
        } 
        catch (error) 
        {
            toast.error("Error en Pago", {description: "Ocurrió un error al procesar el pago. Por favor intente nuevamente.",})
        } 
        finally 
        {
            setIsLoadingSubmit(false)
        }
    }

    // Formatear fecha
    const formatDate = (dateString) => {
        const date = new Date(dateString)
        return date.toLocaleDateString()
    }

    // Obtener estado del préstamo como texto
    const getLoanStatus = (state) => {
        return state === 0 ? "Activo" : "Inactivo"
    }

    return (
        <div className="card-payments-container">
            <div className="card-container">
                
                {/* Botón de regreso */}
                <div className="back-link-container">
                    <Button variant="ghost" className="back-link-client" onClick={() => navigate("/client_dashboard")}>
                        <ArrowLeft className="h-4 w-4" />
                        Volver al Panel
                    </Button>
                </div>

                {/* Titulo y descripcion de la vista original*/}
                <div className="loan-header">
                    <h1 className="page-title">Pagos de Préstamos</h1>
                    <p className="page-description">Realice pagos regulares o extraordinarios en sus préstamos activos</p>
                </div>

                {/* Contenedor de formulario de pago*/}
                <div className="card-payment-card">

                    {/* Titulo y descripcion de la vista de confirmacion*/}
                    <div className="card-payment-header">
                        <h2 className="card-payment-title">{isConfirmStep ? "Confirmar Pago" : "Realizar un Pago"}</h2>
                        <p className="card-payment-description">
                            {isConfirmStep
                                ? "Revise y confirme los detalles de su pago"
                                : "Seleccione un préstamo e ingrese los detalles del pago"}
                        </p>
                    </div>

                    {/* Formulario de pago*/}
                    <form onSubmit={handleSubmit}>
                        {!isConfirmStep ? (
                            <div className="form-content">

                                {/* Selección de prestamo */}
                                <div className="space-y-2">
                                    <Label htmlFor="loanSelect">Seleccionar Préstamo</Label>
                                    <div className="loan-select">
                                        <Select value={selectedLoanId} onValueChange={handleLoanSelect}>
                                            <SelectTrigger id="loanSelect">
                                                <SelectValue placeholder="Elija un préstamo" />
                                            </SelectTrigger>
                                            <SelectContent>
                                                {loans.map((loan) => (
                                                    <SelectItem key={loan.id} value={loan.id.toString()}>
                                                        <div className="flex flex-col">
                                                            <span>Préstamo #{loan.id}</span>
                                                        </div>
                                                    </SelectItem>
                                                ))}
                                            </SelectContent>
                                        </Select>
                                    </div>
                                </div>

                                {/* Detalles del prestamo seleccionado*/}
                                {selectedLoan && (
                                    <div className="loan-details-card">
                                        
                                        {/* Encabezado de prestamos */}
                                        <div className="loan-details-header">
                                            
                                            {/* Menu de prestamos */}
                                            <div className="flex justify-between items-start">
                                                <div>
                                                    <h3 className="font-semibold">Préstamo #{selectedLoan.id}</h3>
                                                    <p className="text-sm text-gray-600">Estado: {getLoanStatus(selectedLoan.state)}</p>
                                                </div>
                                                <Badge variant="outline" className="text-sm">
                                                    Tasa: {selectedLoan.interest_rate}%
                                                </Badge>
                                            </div>
                                        </div>

                                        {/* Contenedor de detalles */}
                                        <div className="loan-details-content">
                                            
                                            {/* Saldo pendiente */}
                                            <div className="loan-details-row">
                                                <span className="loan-details-label">Saldo Pendiente</span>
                                                <span className="loan-details-value">
                                                    {getCurrencySymbol(selectedLoan.currency_id)}{selectedLoan.balance.toFixed(2)}
                                                </span>
                                            </div>

                                            {/* Monto total */}
                                            <div className="loan-details-row">
                                                <span className="loan-details-label">Monto Total</span>
                                                <span className="loan-details-value">
                                                    {getCurrencySymbol(selectedLoan.currency_id) + ' '}
                                                    {selectedLoan.total}
                                                </span>
                                            </div>

                                            {/* Plazo */}
                                            <div className="loan-details-row">
                                                <span className="loan-details-label">Plazo</span>
                                                <span className="loan-details-value">{selectedLoan.lapse} meses</span>
                                            </div>

                                            {/* Fecha de solicitud */}
                                            <div className="loan-details-row">
                                                <span className="loan-details-label">Fecha de Solicitud</span>
                                                <span className="loan-details-value">
                                                    {formatDate(selectedLoan.request_date)}
                                                </span>
                                            </div>

                                        </div>

                                    </div>
                                )}

                                {/* Instrucciones de inicio de vista */}
                                {!selectedLoanId ? (
                                    <div className="text-center py-8">
                                        <PiggyBank className="h-16 w-16 mx-auto text-gray-300 mb-3" />
                                        <p className="text-gray-500">Seleccione un préstamo para realizar un pago</p>
                                    </div>
                                ) : (
                                    <>
                                        {/* Tipo de Pago */}
                                        <div className="space-y-2">
                                            <Label>Tipo de Pago</Label>
                                            <RadioGroup value={paymentType} onValueChange={handlePaymentTypeChange} className="payment-type-radio">
                                                <div className={`payment-type-option ${paymentType === "regular" ? "selected" : ""}`}>
                                                    <div className="flex items-center space-x-2">
                                                        <RadioGroupItem value="regular" id="regular" />
                                                        <Label htmlFor="regular" className="cursor-pointer font-medium">
                                                            Pago Regular
                                                        </Label>
                                                    </div>
                                                    <p className="text-sm text-gray-500 mt-1 ml-6">
                                                        Pago mensual estimado{/*: {getCurrencySymbol(selectedLoan.currency_id) + calculateMonthlyPayment(selectedLoan).toFixed(2)}*/}
                                                    </p>
                                                </div>
                                                <div className={`payment-type-option ${paymentType === "extraordinary" ? "selected" : ""}`}>
                                                    <div className="flex items-center space-x-2">
                                                        <RadioGroupItem value="extraordinary" id="extraordinary" />
                                                        <Label htmlFor="extraordinary" className="cursor-pointer font-medium">
                                                            Pago Extraordinario
                                                        </Label>
                                                    </div>
                                                    <p className="text-sm text-gray-500 mt-1 ml-6">
                                                        Monto personalizado para reducir el saldo principal
                                                    </p>
                                                </div>
                                            </RadioGroup>
                                        </div>

                                        {/* Monto de Pago */}
                                        <div className="space-y-2">
                                            <Label htmlFor="paymentAmount">Monto de Pago</Label>
                                            <div className="relative">
                                                <span className="currency-symbol">
                                                    {selectedAccount ? getCurrencySymbol(selectedAccount.currency_id) : '$'}
                                                </span>
                                                <Input
                                                    id="paymentAmount"
                                                    className="payment-amount-input"
                                                    value={paymentAmount}
                                                    onChange={(e) => setPaymentAmount(e.target.value)}
                                                    placeholder="0.00"
                                                    readOnly={paymentType === "regular"}
                                                />
                                            </div>
                                        </div>

                                        {/* Cuenta de Pago */}
                                        <div className="space-y-2">
                                            <Label htmlFor="paymentAccount">Pagar desde</Label>
                                            <Select value={paymentAccountId} onValueChange={setPaymentAccountId}>
                                                <SelectTrigger id="paymentAccount" className="loan-select">
                                                    <SelectValue placeholder="Seleccione cuenta" />
                                                </SelectTrigger>
                                                <SelectContent>
                                                {/*.filter(account => account.currency_id === selectedLoan?.currency_id) Filtra por misma moneda*/}
                                                    {accounts
                                                        .map(account => (
                                                            <SelectItem key={account.id} value={account.id}>
                                                                <div className="flex justify-between items-center w-full">
                                                                    <span>{account.description}</span>
                                                                    <span className="account-balance">
                                                                        {getCurrencySymbol(account.currency_id)}{account.balance.toFixed(2)}
                                                                    </span>
                                                                </div>
                                                            </SelectItem>
                                                        ))}
                                                </SelectContent>
                                            </Select>
                                        </div>

                                        {/* Fecha de Pago */}
                                        <div className="space-y-2">
                                            <Label>Fecha de Pago</Label>
                                            <div className="payment-date">{currentDate}</div>
                                        </div>
                                    </>
                                )}
                            </div>
                        ) : (
                            /* Paso de Confirmación */
                            <div className="space-y-6">
                                <div className="confirm-section">
                                    
                                    {/* Prestamo */}
                                    <div className="confirm-row">
                                        <span className="confirm-label">Préstamo</span>
                                        <span className="confirm-value">Préstamo #{selectedLoan.id}</span>
                                    </div>

                                    {/* Tipo de pago */}
                                    <div className="confirm-row">
                                        <span className="confirm-label">Tipo de Pago</span>
                                        <span className="confirm-value">
                                            {paymentType === "regular" ? "Pago Regular" : "Pago Extraordinario"}
                                        </span>
                                    </div>

                                    {/* Monto */}
                                    <div className="confirm-row">
                                        <span className="confirm-label">Monto</span>
                                        <span className="confirm-amount">
                                            {getCurrencySymbol(selectedAccount.currency_id) + ' ' + paymentAmount}
                                        </span>
                                    </div>
                                    
                                    {/* Cuenta */}
                                    <div className="confirm-row">
                                        <span className="confirm-label">Desde Cuenta</span>
                                        <span className="confirm-value">{selectedAccount.description}</span>
                                    </div>

                                    {/* Fecha de pago */}
                                    <div className="confirm-row">
                                        <span className="confirm-label">Fecha de Pago</span>
                                        <span className="confirm-value">{currentDate}</span>
                                    </div>

                                </div>

                                {/* Caja de informacion previa al pago */}
                                <div className="info-box">
                                    <div className="flex items-start gap-2">
                                        <Info className="h-5 w-5 text-blue-600 mt-0.5" />
                                        <p className="text-sm text-blue-800">
                                            {paymentType === "extraordinary"
                                                ? "Este pago se aplicará directamente a su saldo principal, lo que puede reducir sus costos de interés totales."
                                                : "Este es su pago mensual estimado según los términos del préstamo."}
                                        </p>
                                    </div>
                                </div>

                            </div>
                        )}

                        {/* Botones del formulario */}
                        <div className="flex justify-between mt-8">
                            {isConfirmStep && (
                                <button type="button" className="back-button" onClick={() => setIsConfirmStep(false)}>
                                    Atrás
                                </button>
                            )}
                            <button type="submit" className={`submit-button ${isConfirmStep ? "" : "w-full"}`} disabled={!selectedLoanId || isLoading}>
                                {isLoading ? "Procesando..." : isConfirmStep ? "Confirmar Pago" : "Continuar"}
                            </button>
                        </div>

                    </form>

                </div>
            </div>
            <Toaster position="bottom-right" />
        </div>
    )
}

export default LoanPayments