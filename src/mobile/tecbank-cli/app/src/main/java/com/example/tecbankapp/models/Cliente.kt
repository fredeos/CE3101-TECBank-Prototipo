
package com.example.tecbankapp.models

data class Cliente(
    val id: String? = null,
    val nombreCompleto: String,
    val cedula: String,
    val direccion: String,
    val telefono: String,
    val ingresoMensual: Double,
    val tipoCliente: Int,
    val usuario: String,
    val password: String
)
