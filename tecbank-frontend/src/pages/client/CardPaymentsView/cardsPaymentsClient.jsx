"use client"

import { useState } from "react"
import { useNavigate } from "react-router-dom"
import { toast, Toaster } from "sonner"
import { ArrowLeft, Info, CheckCircle } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Checkbox } from "@/components/ui/checkbox"
import "./cardsPaymentsStyle.css"

// Datos de ejemplo para tarjetas de crédito
const creditCards = [
{
    id: "1",
    name: "Tarjeta de Recompensas Platinum",
    number: "**** **** **** 5678",
    expiryDate: "05/25",
    availableCredit: 8500,
    totalLimit: 10000,
    balance: 1500,
    minPayment: 35,
    dueDate: "2023-05-18",
    apr: 18.99,
    issuer: "Visa",
    color: "card-platinum",
},
{
    id: "2",
    name: "Mastercard de Reembolso",
    number: "**** **** **** 1234",
    expiryDate: "09/24",
    availableCredit: 3200,
    totalLimit: 5000,
    balance: 1800,
    minPayment: 45,
    dueDate: "2023-05-22",
    apr: 15.99,
    issuer: "Mastercard",
    color: "card-blue",
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

function CardPayments() {

    const navigate = useNavigate()

    // Obtener fecha actual
    const currentDate = new Date().toLocaleDateString()

    // Estado para tarjeta seleccionada y detalles de pago
    const [selectedCardId, setSelectedCardId] = useState("")
    const [paymentAmount, setPaymentAmount] = useState("")
    const [paymentType, setPaymentType] = useState("minimum")
    const [paymentAccountId, setPaymentAccountId] = useState("")
    const [isRecurring, setIsRecurring] = useState(false)
    const [isConfirmStep, setIsConfirmStep] = useState(false)
    const [isLoading, setIsLoading] = useState(false)
    const [isSuccess, setIsSuccess] = useState(false)

    // Obtener detalles de la tarjeta y cuenta seleccionadas
    const selectedCard = creditCards.find((card) => card.id === selectedCardId)
    const selectedAccount = paymentAccounts.find((account) => account.id === paymentAccountId)

    // Manejar selección de tarjeta
    const handleCardSelect = (cardId) => {
        const card = creditCards.find((card) => card.id === cardId)
        setSelectedCardId(cardId)
        setPaymentAmount(card.minPayment.toFixed(2))
        setPaymentType("minimum")
        setIsConfirmStep(false)
        setIsSuccess(false)
    }

    // Manejar cambio de tipo de pago
    const handlePaymentTypeChange = (value) => {
        setPaymentType(value)
        if (value === "minimum" && selectedCard) {
        setPaymentAmount(selectedCard.minPayment.toFixed(2))
        } else if (value === "full" && selectedCard) {
        setPaymentAmount(selectedCard.balance.toFixed(2))
        } else if (value === "custom") {
        setPaymentAmount("")
        }
    }

    // Manejar envío del formulario
    const handleSubmit = (e) => {
        e.preventDefault()

        // Validar formulario
        if (!selectedCardId || !paymentAmount || !paymentAccountId) {
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

        if (Number.parseFloat(paymentAmount) > selectedCard.balance) {
        toast.error("Pago Demasiado Grande", {
            description: "El monto del pago no puede exceder el saldo actual.",
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
        setIsSuccess(true)
        setIsConfirmStep(false)

        toast.success("Pago Exitoso", {
            description: `El pago de $${Number.parseFloat(paymentAmount).toFixed(2)} ha sido procesado exitosamente.`,
        })
        }, 1500)
    }

    // Reiniciar formulario para hacer otro pago
    const handleReset = () => {
        setSelectedCardId("")
        setPaymentAmount("")
        setPaymentType("minimum")
        setPaymentAccountId("")
        setIsRecurring(false)
        setIsSuccess(false)
    }

    return (
        <div className="card-payments-container">
        <div className="card-container">
            {/* Botón de regreso */}
            <div className="back-link-container">
            <Button variant="ghost" className="back-link-client" onClick={() => navigate("/client_dashboard")}>
                <ArrowLeft className="back-icon" />
                <span>Volver al Panel</span>
            </Button>
            </div>

            <div className="card-header">
            <h1 className="page-title">Pagos de Tarjeta de Crédito</h1>
            <p className="page-description">Realice un pago a su tarjeta de crédito</p>
            </div>

            {isSuccess ? (
            <div className="success-card">
                <div className="success-icon-container">
                <CheckCircle className="success-icon-check" />
                </div>
                <h2 className="success-title">¡Pago Exitoso!</h2>
                <p className="success-message">
                Su pago de ${Number.parseFloat(paymentAmount).toFixed(2)} a {selectedCard.name} ha sido procesado
                exitosamente.
                </p>
                <div className="success-details">
                <div className="success-detail-row">
                    <span className="success-label">Tarjeta</span>
                    <span className="success-value">{selectedCard.name}</span>
                </div>
                <div className="success-detail-row">
                    <span className="success-label">Monto</span>
                    <span className="success-value">${Number.parseFloat(paymentAmount).toFixed(2)}</span>
                </div>
                <div className="success-detail-row">
                    <span className="success-label">Desde Cuenta</span>
                    <span className="success-value">{selectedAccount.name}</span>
                </div>
                <div className="success-detail-row">
                    <span className="success-label">Fecha</span>
                    <span className="success-value">{currentDate}</span>
                </div>
                {isRecurring && (
                    <div className="success-detail-row">
                    <span className="success-label">Recurrente</span>
                    <span className="success-value">Pago mensual en fecha de vencimiento</span>
                    </div>
                )}
                </div>
                <Button onClick={handleReset} className="another-payment-button">
                Realizar Otro Pago
                </Button>
            </div>
            ) : (
            <div className="card-payment-card">
                <div className="card-payment-header">
                <h2 className="card-payment-title">{isConfirmStep ? "Confirmar Pago" : "Realizar un Pago"}</h2>
                <p className="card-payment-description">
                    {isConfirmStep
                    ? "Revise y confirme los detalles de su pago"
                    : "Seleccione una tarjeta e ingrese los detalles del pago"}
                </p>
                </div>

                <form onSubmit={handleSubmit}>
                {!isConfirmStep ? (
                    <div className="form-content">
                    {/* Selección de Tarjeta */}
                    <div className="form-group-client">
                        <Label htmlFor="cardSelect" className="form-label">
                        Seleccionar Tarjeta
                        </Label>
                        <div className="card-select">
                        <Select value={selectedCardId} onValueChange={handleCardSelect}>
                            <SelectTrigger id="cardSelect">
                            <SelectValue placeholder="Elija una tarjeta de crédito" />
                            </SelectTrigger>
                            <SelectContent>
                            {creditCards.map((card) => (
                                <SelectItem key={card.id} value={card.id}>
                                {card.name}
                                </SelectItem>
                            ))}
                            </SelectContent>
                        </Select>
                        </div>
                    </div>

                    {/* Detalles de la Tarjeta Seleccionada */}
                    {selectedCard && (
                        <div className={`credit-card-display ${selectedCard.color}`}>
                        <div className="credit-card-content">
                            <div className="credit-card-issuer">{selectedCard.issuer}</div>
                            <div className="credit-card-number">{selectedCard.number}</div>
                            <div className="credit-card-details">
                            <div>
                                <div className="credit-card-label">VÁLIDO HASTA</div>
                                <div>{selectedCard.expiryDate}</div>
                            </div>
                            <div>
                                <div className="credit-card-label">NOMBRE</div>
                                <div>Juan Pérez</div>
                            </div>
                            </div>
                        </div>
                        <div className="credit-card-balance-section">
                            <div className="credit-card-balance-row">
                            <span>Saldo Actual</span>
                            <span className="balance-amount">${selectedCard.balance.toFixed(2)}</span>
                            </div>
                            <div className="credit-card-balance-row">
                            <span>Pago Mínimo</span>
                            <span>${selectedCard.minPayment.toFixed(2)}</span>
                            </div>
                            <div className="credit-card-balance-row">
                            <span>Fecha de Vencimiento</span>
                            <span>{new Date(selectedCard.dueDate).toLocaleDateString()}</span>
                            </div>
                            <div className="credit-card-balance-row">
                            <span>Crédito Disponible</span>
                            <span>${selectedCard.availableCredit.toFixed(2)}</span>
                            </div>
                        </div>
                        </div>
                    )}

                    {selectedCardId && (
                        <>
                        {/* Tipo de Pago */}
                        <div className="form-group-client">
                            <Label className="form-label">Monto de Pago</Label>
                            <div className="payment-options">
                            <button
                                type="button"
                                className={`payment-option ${paymentType === "minimum" ? "selected" : ""}`}
                                onClick={() => handlePaymentTypeChange("minimum")}
                            >
                                <span className="payment-option-label">Pago Mínimo</span>
                                <span className="payment-option-amount">${selectedCard.minPayment.toFixed(2)}</span>
                            </button>
                            <button
                                type="button"
                                className={`payment-option ${paymentType === "full" ? "selected" : ""}`}
                                onClick={() => handlePaymentTypeChange("full")}
                            >
                                <span className="payment-option-label">Saldo Completo</span>
                                <span className="payment-option-amount">${selectedCard.balance.toFixed(2)}</span>
                            </button>
                            <button
                                type="button"
                                className={`payment-option ${paymentType === "custom" ? "selected" : ""}`}
                                onClick={() => handlePaymentTypeChange("custom")}
                            >
                                <span className="payment-option-label">Monto Personalizado</span>
                            </button>
                            </div>
                        </div>

                        {/* Monto de Pago (para personalizado) */}
                        {paymentType === "custom" && (
                            <div className="form-group-client">
                            <Label htmlFor="paymentAmount" className="form-label">
                                Ingresar Monto
                            </Label>
                            <div className="input-container">
                                <span className="currency-symbol">$</span>
                                <Input
                                id="paymentAmount"
                                className="payment-amount-input"
                                value={paymentAmount}
                                onChange={(e) => setPaymentAmount(e.target.value)}
                                placeholder="0.00"
                                />
                            </div>
                            </div>
                        )}

                        {/* Cuenta de Pago */}
                        <div className="form-group-client">
                            <Label htmlFor="paymentAccount" className="form-label">
                            Pagar Desde
                            </Label>
                            <Select value={paymentAccountId} onValueChange={setPaymentAccountId}>
                            <SelectTrigger id="paymentAccount" className="card-select">
                                <SelectValue placeholder="Seleccione cuenta" />
                            </SelectTrigger>
                            <SelectContent>
                                {paymentAccounts.map((account) => (
                                <SelectItem key={account.id} value={account.id}>
                                    <div className="account-option">
                                    <span>{account.name}</span>
                                    <span className="account-balance">${account.balance.toFixed(2)}</span>
                                    </div>
                                </SelectItem>
                                ))}
                            </SelectContent>
                            </Select>
                        </div>

                        {/* Fecha de Pago (Fija a la fecha actual) */}
                        <div className="form-group-client">
                            <Label className="form-label">Fecha de Pago</Label>
                            <div className="payment-date">{currentDate}</div>
                        </div>

                        {/* Opción de Pago Recurrente */}
                        <div className="recurring-container">
                            <Checkbox
                            id="recurring"
                            checked={isRecurring}
                            onCheckedChange={setIsRecurring}
                            className="recurring-checkbox"
                            />
                            <div className="recurring-text">
                            <Label htmlFor="recurring" className="recurring-label">
                                Hacer de este un pago recurrente
                            </Label>
                            <p className="recurring-description">
                                Pagaremos automáticamente el monto mínimo adeudado cada mes en la fecha de vencimiento.
                            </p>
                            </div>
                        </div>
                        </>
                    )}
                    </div>
                ) : (
                    /* Paso de Confirmación */
                    <div className="confirm-container">
                    <div className="confirm-section">
                        <div className="confirm-row">
                        <span className="confirm-label">Tarjeta</span>
                        <span className="confirm-value">{selectedCard.name}</span>
                        </div>
                        <div className="confirm-row">
                        <span className="confirm-label">Número de Tarjeta</span>
                        <span className="confirm-value">{selectedCard.number}</span>
                        </div>
                        <div className="confirm-row">
                        <span className="confirm-label">Monto de Pago</span>
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
                        {isRecurring && (
                        <div className="confirm-row">
                            <span className="confirm-label">Pago Recurrente</span>
                            <span className="confirm-value">Sí, mensualmente en fecha de vencimiento</span>
                        </div>
                        )}
                    </div>

                    <div className="info-box">
                        <div className="info-content">
                        <Info className="info-icon" />
                        <p className="info-text">
                            {paymentType === "minimum"
                            ? "Realizar solo el pago mínimo resultará en pagar más intereses a lo largo del tiempo."
                            : paymentType === "full"
                                ? "Pagar su saldo completo le ayuda a evitar cargos por intereses en compras."
                                : "Los pagos personalizados le permiten pagar un monto que se ajuste a su presupuesto."}
                        </p>
                        </div>
                    </div>
                    </div>
                )}

                <div className="form-actions">
                    {isConfirmStep && (
                    <button type="button" className="back-button" onClick={() => setIsConfirmStep(false)}>
                        Atrás
                    </button>
                    )}
                    <button
                    type="submit"
                    className={`submit-button ${isConfirmStep ? "" : "full-width"}`}
                    disabled={!selectedCardId || isLoading}
                    >
                    {isLoading ? "Procesando..." : isConfirmStep ? "Confirmar Pago" : "Continuar"}
                    </button>
                </div>
                </form>
            </div>
            )}
        </div>
        <Toaster position="top-center" />
        </div>
    )
}

export default CardPayments
