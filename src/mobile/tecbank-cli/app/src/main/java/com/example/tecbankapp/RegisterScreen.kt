@file:OptIn(ExperimentalMaterial3Api::class)

package com.example.tecbankapp
import com.example.tecbankapp.models.Cliente

import com.example.tecbankapp.network.RetrofitInstance
import android.util.Log
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.navigation.NavHostController
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext

@Composable
fun RegisterScreen(
    navController: NavHostController,
    onRegisterSuccess: () -> Unit
) {
    // Campos del formulario
    var idCard by remember { mutableStateOf("") }
    var name by remember { mutableStateOf("") }
    var lastName by remember { mutableStateOf("") }
    var username by remember { mutableStateOf("") }
    var password by remember { mutableStateOf("") }
    var income by remember { mutableStateOf("") }
    var phone by remember { mutableStateOf("") }
    var address by remember { mutableStateOf("") }

    // Dropdown para el tipo
    var expanded by remember { mutableStateOf(false) }
    var selectedTypeText by remember { mutableStateOf("Físico") }
    val typeValue = if (selectedTypeText == "Físico") 1 else 2
    val typeOptions = listOf("Físico", "Jurídico")

    Column(
        modifier = Modifier
            .fillMaxSize()
            .verticalScroll(rememberScrollState())
            .padding(16.dp),
        verticalArrangement = Arrangement.spacedBy(12.dp)
    ) {
        Text("Registration", fontSize = 22.sp, modifier = Modifier.align(Alignment.CenterHorizontally))

        OutlinedTextField(value = idCard, onValueChange = { idCard = it }, label = { Text("Id Card:") })
        OutlinedTextField(value = name, onValueChange = { name = it }, label = { Text("Name:") })
        OutlinedTextField(value = lastName, onValueChange = { lastName = it }, label = { Text("Last name:") })
        OutlinedTextField(value = username, onValueChange = { username = it }, label = { Text("Username:") })
        OutlinedTextField(value = password, onValueChange = { password = it }, label = { Text("Password:") }, visualTransformation = PasswordVisualTransformation())
        OutlinedTextField(value = income, onValueChange = { income = it }, label = { Text("Monthly Income:") })
        OutlinedTextField(value = phone, onValueChange = { phone = it }, label = { Text("Phone Number:") })
        OutlinedTextField(value = address, onValueChange = { address = it }, label = { Text("Address:") })

        // Dropdown material 3 moderno
        ExposedDropdownMenuBox(
            expanded = expanded,
            onExpandedChange = { expanded = !expanded }
        ) {
            OutlinedTextField(
                modifier = Modifier
                    .menuAnchor()
                    .fillMaxWidth(),
                readOnly = true,
                value = selectedTypeText,
                onValueChange = {},
                label = { Text("Type") },
                trailingIcon = {
                    ExposedDropdownMenuDefaults.TrailingIcon(expanded = expanded)
                },
                colors = ExposedDropdownMenuDefaults.textFieldColors()
            )
            ExposedDropdownMenu(
                expanded = expanded,
                onDismissRequest = { expanded = false }
            ) {
                typeOptions.forEach { selectionOption ->
                    DropdownMenuItem(
                        text = { Text(selectionOption) },
                        onClick = {
                            selectedTypeText = selectionOption
                            expanded = false
                        }
                    )
                }
            }
        }

        // Botones
        Row(
            modifier = Modifier.fillMaxWidth(),
            horizontalArrangement = Arrangement.SpaceBetween
        ) {
            Button(onClick = {
                val nuevoCliente = Cliente(
                    nombreCompleto = "$name $lastName",
                    cedula = idCard,
                    direccion = address,
                    telefono = phone,
                    ingresoMensual = income.toDoubleOrNull() ?: 0.0,
                    tipoCliente = if (selectedTypeText == "Físico") 1 else 2,
                    usuario = username,
                    password = password
                )

                CoroutineScope(Dispatchers.IO).launch {
                    try {
                        val response = RetrofitInstance.api.registrarCliente(nuevoCliente)
                        if (response.isSuccessful) {
                            withContext(Dispatchers.Main) {
                                onRegisterSuccess()
                            }
                        } else {
                            Log.e("API", "Error al registrar: ${response.code()}")
                        }
                    } catch (e: Exception) {
                        Log.e("API", "Excepción: ${e.message}")
                    }
                }
            }) {
                Text("Sign up")
            }


            Button(onClick = {
                navController.popBackStack()
            }) {
                Text("Back")
            }
        }
    }
}
