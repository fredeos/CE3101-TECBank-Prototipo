"use client"

import { useState } from "react"
import { useNavigate } from "react-router-dom"
import { toast, Toaster } from "sonner"
import { ArrowLeft, Info, PiggyBank } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { RadioGroup, RadioGroupItem } from "@/components/ui/radio-group"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Badge } from "@/components/ui/badge"
import "./loansPaymentStyle.css"

import { loans } from "@/mocks/clientMocks/clientLoans"
import { sourceAccounts } from "@/mocks/clientMocks/clientAccounts"

// Datos de ejemplo para cuentas de pago
const paymentAccounts = [
    {
        id: "1",
        name: "Cuenta Corriente Principal",
        number: "**** 4567",
        balance: 3250.75,
    },
    {
        id: "2",
        name: "Cuenta de Ahorros",
        number: "**** 8901",
        balance: 12500.5,
    },
]



function LoanPayments() {

    const navigate = useNavigate()
    const currentDate = new Date().toLocaleDateString()

    // Estado para préstamo seleccionado y detalles de pago
    const [selectedLoanId, setSelectedLoanId] = useState("")
    const [paymentType, setPaymentType] = useState("regular")
    const [paymentAmount, setPaymentAmount] = useState("")
    const [paymentAccountId, setPaymentAccountId] = useState("")
    const [isConfirmStep, setIsConfirmStep] = useState(false)
    const [isLoading, setIsLoading] = useState(false)

    // Obtener detalles del préstamo y cuenta seleccionados
    const selectedLoan = loans.find((loan) => loan.id.toString() === selectedLoanId)
    const selectedAccount = sourceAccounts.find((account) => account.id === paymentAccountId)

    // Calcular pago mensual estimado
    const calculateMonthlyPayment = (loan) => {
        if (!loan) return 0
        // Fórmula simplificada para calcular pago mensual
        const monthlyRate = loan.interest_rate / 100 / 12
        const payment = (loan.balance * monthlyRate) / (1 - Math.pow(1 + monthlyRate, -loan.lapse))
        return payment
    }

    // Manejar selección de préstamo
    const handleLoanSelect = (loanId) => {
        const loan = loans.find((loan) => loan.id.toString() === loanId)
        setSelectedLoanId(loanId)
        setPaymentAmount(calculateMonthlyPayment(loan).toFixed(2))
        setPaymentType("regular")
        setIsConfirmStep(false)
    }

    // Manejar cambio de tipo de pago
    const handlePaymentTypeChange = (value) => {
        setPaymentType(value)
        if (value === "regular" && selectedLoan) {
            setPaymentAmount(calculateMonthlyPayment(selectedLoan).toFixed(2))
        } else if (value === "extraordinary") {
            setPaymentAmount("")
        }
    }

    // Manejo del cambio de moneda
    const getCurrencySymbol = (currencyId) => {
        const currencySymbols = {
            1: '$', // Dólar
            2: '€', // Euro
            3: '₡'  // Colón costarricense
        };
        return currencySymbols[currencyId] || '$'; // Default a dólar si no se encuentra
    };

    // Manejar envío del formulario
    const handleSubmit = (e) => {
        e.preventDefault()

        // Validar formulario
        if (!selectedLoanId || !paymentAmount || !paymentAccountId) {
            toast.error("Información Faltante", {
                description: "Por favor complete todos los campos requeridos para continuar.",
            })
            return
        }

        if (isNaN(paymentAmount) || Number.parseFloat(paymentAmount) <= 0) {
            toast.error("Monto Inválido", {
                description: "Por favor ingrese un monto de pago válido.",
            })
            return
        }

        if (selectedAccount && Number.parseFloat(paymentAmount) > selectedAccount.balance) {
            toast.error("Fondos Insuficientes", {
                description: "El saldo de su cuenta es insuficiente para este pago.",
            })
            return
        }

        if (selectedLoan && Number.parseFloat(paymentAmount) > selectedLoan.balance) {
            toast.error("Monto Excede Saldo", {
                description: "El monto del pago no puede exceder el saldo pendiente del préstamo.",
            })
            return
        }

        // Si la validación pasa y no está en paso de confirmación, mostrar confirmación
        if (!isConfirmStep) {
            setIsConfirmStep(true)
            return
        }

        // Procesar pago
        setIsLoading(true)

        // Simular llamada a API
        setTimeout(() => {
            setIsLoading(false)
            toast.success("Pago Exitoso", {
                description: `El pago de ${getCurrencySymbol(selectedLoan.currency_id) + ' ' + paymentAmount} ha sido procesado exitosamente.`,
            })

            // Reiniciar formulario
            setIsConfirmStep(false)
            setSelectedLoanId("")
            setPaymentType("regular")
            setPaymentAmount("")
            setPaymentAccountId("")
        }, 1500)
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
        <div className="loan-payments-container">
            <div className="max-w-3xl mx-auto p-6">
                {/* Botón de regreso */}
                <div className="mb-6">
                    <Button variant="ghost" className="back-link-client" onClick={() => navigate("/client_dashboard")}>
                        <ArrowLeft className="h-4 w-4" />
                        Volver al Panel
                    </Button>
                </div>

                <div className="loan-header">
                    <h1 className="text-2xl font-bold mb-2">Pagos de Préstamos</h1>
                    <p className="text-gray-600">Realice pagos regulares o extraordinarios en sus préstamos activos</p>
                </div>

                <div className="loan-card bg-white p-6">
                    <div className="mb-4">
                        <h2 className="text-xl font-semibold mb-2">{isConfirmStep ? "Confirmar Pago" : "Realizar un Pago"}</h2>
                        <p className="text-gray-500 text-sm">
                            {isConfirmStep
                                ? "Revise y confirme los detalles de su pago"
                                : "Seleccione un préstamo e ingrese los detalles del pago"}
                        </p>
                    </div>

                    <form onSubmit={handleSubmit}>
                        {!isConfirmStep ? (
                            <div className="space-y-6">
                                {/* Selección de Préstamo */}
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

                                {/* Detalles del Préstamo Seleccionado */}
                                {selectedLoan && (
                                    <div className="loan-details-card">
                                        <div className="loan-details-header">
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
                                        <div className="loan-details-content">
                                            <div className="loan-details-row">
                                                <span className="loan-details-label">Saldo Pendiente</span>
                                                <span className="loan-details-value">
                                                    {getCurrencySymbol(selectedLoan.currency_id) + ' '}
                                                    {selectedLoan.balance}
                                                </span>
                                            </div>
                                            <div className="loan-details-row">
                                                <span className="loan-details-label">Monto Total</span>
                                                <span className="loan-details-value">
                                                    {getCurrencySymbol(selectedLoan.currency_id) + ' '}
                                                    {selectedLoan.total}
                                                </span>
                                            </div>
                                            <div className="loan-details-row">
                                                <span className="loan-details-label">Plazo</span>
                                                <span className="loan-details-value">{selectedLoan.lapse} meses</span>
                                            </div>
                                            <div className="loan-details-row">
                                                <span className="loan-details-label">Fecha de Solicitud</span>
                                                <span className="loan-details-value">
                                                    {formatDate(selectedLoan.request_date)}
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                )}

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
                                            <RadioGroup
                                                value={paymentType}
                                                onValueChange={handlePaymentTypeChange}
                                                className="payment-type-radio"
                                            >
                                                <div className={`payment-type-option ${paymentType === "regular" ? "selected" : ""}`}>
                                                    <div className="flex items-center space-x-2">
                                                        <RadioGroupItem value="regular" id="regular" />
                                                        <Label htmlFor="regular" className="cursor-pointer font-medium">
                                                            Pago Regular
                                                        </Label>
                                                    </div>
                                                    <p className="text-sm text-gray-500 mt-1 ml-6">
                                                        Pago mensual estimado: {getCurrencySymbol(selectedLoan.currency_id) + calculateMonthlyPayment(selectedLoan).toFixed(2)}
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
                                                    {selectedLoan ? getCurrencySymbol(selectedLoan.currency_id) : '$'}
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
                                            {paymentType === "extraordinary" && (
                                                <p className="text-xs text-gray-500">
                                                    Los pagos extraordinarios se aplican directamente a su saldo principal.
                                                </p>
                                            )}
                                        </div>

                                        {/* Cuenta de Pago */}
                                        <div className="space-y-2">
                                            <Label htmlFor="paymentAccount">Pagar desde</Label>
                                            <Select value={paymentAccountId} onValueChange={setPaymentAccountId}>
                                                <SelectTrigger id="paymentAccount" className="loan-select">
                                                    <SelectValue placeholder="Seleccione cuenta" />
                                                </SelectTrigger>
                                                <SelectContent>
                                                    {/*.filter(account => account.currency_id === selectedLoan?.currency_id)*/}
                                                    {sourceAccounts
                                                        
                                                        .map(account => (
                                                            <SelectItem key={account.id} value={account.id}>
                                                                <div className="flex justify-between items-center w-full">
                                                                    <span>{account.description + ': ' + (account.id)}</span>
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
                                    <div className="confirm-row">
                                        <span className="confirm-label">Préstamo</span>
                                        <span className="confirm-value">Préstamo #{selectedLoan.id}</span>
                                    </div>

                                    <div className="confirm-row">
                                        <span className="confirm-label">Tipo de Pago</span>
                                        <span className="confirm-value">
                                            {paymentType === "regular" ? "Pago Regular" : "Pago Extraordinario"}
                                        </span>
                                    </div>

                                    <div className="confirm-row">
                                        <span className="confirm-label">Monto</span>
                                        <span className="confirm-amount">
                                            {getCurrencySymbol(selectedLoan.currency_id) + ' ' + paymentAmount}
                                        </span>
                                    </div>

                                    <div className="confirm-row">
                                        <span className="confirm-label">Desde Cuenta</span>
                                        <span className="confirm-value">{selectedAccount.description}</span>
                                    </div>

                                    <div className="confirm-row">
                                        <span className="confirm-label">Fecha de Pago</span>
                                        <span className="confirm-value">{currentDate}</span>
                                    </div>
                                </div>

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

                        <div className="flex justify-between mt-8">
                            {isConfirmStep && (
                                <button type="button" className="back-button" onClick={() => setIsConfirmStep(false)}>
                                    Atrás
                                </button>
                            )}
                            <button
                                type="submit"
                                className={`submit-button ${isConfirmStep ? "" : "w-full"}`}
                                disabled={!selectedLoanId || isLoading}
                            >
                                {isLoading ? "Procesando..." : isConfirmStep ? "Confirmar Pago" : "Continuar"}
                            </button>
                        </div>
                    </form>
                </div>
            </div>
            <Toaster position="top-center" />
        </div>
    )
}

export default LoanPayments