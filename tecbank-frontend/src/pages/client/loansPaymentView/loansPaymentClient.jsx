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

// Datos de ejemplo para préstamos activos
const activeLoans = [
{
    id: "1",
    name: "Hipoteca de Vivienda",
    type: "Hipoteca",
    originalAmount: 250000,
    outstandingBalance: 175000,
    monthlyPayment: 1250.75,
    nextPaymentDate: "2023-05-15",
    status: "current",
},
{
    id: "2",
    name: "Préstamo de Auto",
    type: "Auto",
    originalAmount: 35000,
    outstandingBalance: 18500,
    monthlyPayment: 525.3,
    nextPaymentDate: "2023-05-10",
    status: "current",
},
]

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

    // Obtener fecha actual
    const currentDate = new Date().toLocaleDateString()

    // Estado para préstamo seleccionado y detalles de pago
    const [selectedLoanId, setSelectedLoanId] = useState("")
    const [paymentType, setPaymentType] = useState("regular")
    const [paymentAmount, setPaymentAmount] = useState("")
    const [paymentAccountId, setPaymentAccountId] = useState("")
    const [isConfirmStep, setIsConfirmStep] = useState(false)
    const [isLoading, setIsLoading] = useState(false)

    // Obtener detalles del préstamo y cuenta seleccionados
    const selectedLoan = activeLoans.find((loan) => loan.id === selectedLoanId)
    const selectedAccount = paymentAccounts.find((account) => account.id === paymentAccountId)

    // Manejar selección de préstamo
    const handleLoanSelect = (loanId) => {
        const loan = activeLoans.find((loan) => loan.id === loanId)
        setSelectedLoanId(loanId)
        setPaymentAmount(loan.monthlyPayment.toFixed(2))
        setPaymentType("regular")
        setIsConfirmStep(false)
    }

    // Manejar cambio de tipo de pago
    const handlePaymentTypeChange = (value) => {
        setPaymentType(value)
        if (value === "regular" && selectedLoan) {
        setPaymentAmount(selectedLoan.monthlyPayment.toFixed(2))
        } else if (value === "extraordinary") {
        setPaymentAmount("")
        }
    }

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
            description: `El pago de ${Number.parseFloat(paymentAmount).toFixed(2)} ha sido procesado exitosamente.`,
        })

        // Reiniciar formulario
        setIsConfirmStep(false)
        setSelectedLoanId("")
        setPaymentType("regular")
        setPaymentAmount("")
        setPaymentAccountId("")
        }, 1500)
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
                            {activeLoans.map((loan) => (
                            <SelectItem key={loan.id} value={loan.id}>
                                {loan.name}
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
                            <h3 className="font-semibold">{selectedLoan.name}</h3>
                            <p className="text-sm text-gray-600">Préstamo de {selectedLoan.type}</p>
                            </div>
                            <Badge
                            variant={selectedLoan.status === "current" ? "outline" : "destructive"}
                            className="bg-green-50 text-green-700 border-green-200"
                            >
                            {selectedLoan.status === "current" ? "Al Día" : "Vencido"}
                            </Badge>
                        </div>
                        </div>
                        <div className="loan-details-content">
                        <div className="loan-details-row">
                            <span className="loan-details-label">Saldo Pendiente</span>
                            <span className="loan-details-value">${selectedLoan.outstandingBalance.toLocaleString()}</span>
                        </div>
                        <div className="loan-details-row">
                            <span className="loan-details-label">Pago Mensual</span>
                            <span className="loan-details-value">${selectedLoan.monthlyPayment.toFixed(2)}</span>
                        </div>
                        <div className="loan-details-row">
                            <span className="loan-details-label">Próximo Pago</span>
                            <span className="loan-details-value">
                            {new Date(selectedLoan.nextPaymentDate).toLocaleDateString()}
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
                                Pago mensual estándar: ${selectedLoan.monthlyPayment.toFixed(2)}
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
                            <span className="currency-symbol">$</span>
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
                        <Label htmlFor="paymentAccount">Pagar Desde</Label>
                        <Select value={paymentAccountId} onValueChange={setPaymentAccountId}>
                            <SelectTrigger id="paymentAccount" className="loan-select">
                            <SelectValue placeholder="Seleccione cuenta" />
                            </SelectTrigger>
                            <SelectContent>
                            {paymentAccounts.map((account) => (
                                <SelectItem key={account.id} value={account.id}>
                                <div className="flex justify-between items-center w-full">
                                    <span>{account.name}</span>
                                    <span className="text-gray-500 text-sm">${account.balance.toFixed(2)}</span>
                                </div>
                                </SelectItem>
                            ))}
                            </SelectContent>
                        </Select>
                        </div>

                        {/* Fecha de Pago (Fija a la fecha actual) */}
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
                        <span className="confirm-value">{selectedLoan.name}</span>
                    </div>

                    <div className="confirm-row">
                        <span className="confirm-label">Tipo de Pago</span>
                        <span className="confirm-value">
                        {paymentType === "regular" ? "Pago Regular" : "Pago Extraordinario"}
                        </span>
                    </div>

                    <div className="confirm-row">
                        <span className="confirm-label">Monto</span>
                        <span className="confirm-amount">${Number.parseFloat(paymentAmount).toFixed(2)}</span>
                    </div>

                    <div className="confirm-row">
                        <span className="confirm-label">Desde Cuenta</span>
                        <span className="confirm-value">{selectedAccount.name}</span>
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
                            : "Este es su pago mensual regular según lo programado."}
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