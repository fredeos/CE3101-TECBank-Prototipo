@file:OptIn(ExperimentalMaterial3Api::class)
package com.example.tecbankapp

import androidx.compose.foundation.layout.*
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import org.json.JSONObject
import java.io.BufferedReader
import java.io.InputStreamReader
import java.net.HttpURLConnection
import java.net.URL
import android.util.Log
import kotlinx.coroutines.withContext
import kotlinx.coroutines.Dispatchers
import androidx.compose.ui.*
import androidx.compose.ui.text.input.*
import androidx.compose.ui.unit.*
import androidx.navigation.NavHostController
import kotlinx.coroutines.*
import java.io.*
import java.net.*
import androidx.compose.ui.text.style.TextDecoration
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.foundation.clickable
import androidx.compose.foundation.verticalScroll
import androidx.compose.runtime.remember
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll



@Composable
fun SignUpScreen(navController: NavHostController) {
    val scrollState = rememberScrollState()
    var username by remember { mutableStateOf("") }
    var password by remember { mutableStateOf("") }
    var name by remember { mutableStateOf("") }
    var lastName1 by remember { mutableStateOf("") }
    var lastName2 by remember { mutableStateOf("") }
    var clientType by remember { mutableStateOf("") }
    var income by remember { mutableStateOf("") }
    var phone by remember { mutableStateOf("") }
    var address by remember { mutableStateOf("") }
    var errorMessage by remember { mutableStateOf("") }
    var id by remember { mutableStateOf("") }


    Column(
        modifier = Modifier
            .fillMaxSize()
            .padding(24.dp)
            .verticalScroll(rememberScrollState()),
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.Center
    ) {
        Text("Create a New Account", style = MaterialTheme.typography.titleLarge)

        Spacer(modifier = Modifier.height(16.dp))

        OutlinedTextField(
            value = id,
            onValueChange = { id = it },
            label = { Text("Cédula (ID)") }
        )


        OutlinedTextField(value = name, onValueChange = { name = it }, label = { Text("Name") })
        OutlinedTextField(value = lastName1, onValueChange = { lastName1 = it }, label = { Text("Last Name 1") })
        OutlinedTextField(value = lastName2, onValueChange = { lastName2 = it }, label = { Text("Last Name 2") })
        OutlinedTextField(value = clientType, onValueChange = { clientType = it }, label = { Text("Tipo de cliente (físico/jurídico)") })
        OutlinedTextField(value = username, onValueChange = { username = it }, label = { Text("Username") })
        OutlinedTextField(value = password, onValueChange = { password = it }, label = { Text("Password") }, visualTransformation = PasswordVisualTransformation())
        OutlinedTextField(value = income, onValueChange = { income = it }, label = { Text("Monthly Income") })
        OutlinedTextField(value = phone, onValueChange = { phone = it }, label = { Text("Phone Number") })
        OutlinedTextField(value = address, onValueChange = { address = it }, label = { Text("Address") })

        Spacer(modifier = Modifier.height(24.dp))

        Button(onClick = {
            CoroutineScope(Dispatchers.IO).launch {
                try {
                    val typeInt = when (clientType.trim().lowercase()) {
                        "físico", "fisico" -> 1
                        "jurídico", "juridico" -> 2
                        else -> {
                            withContext(Dispatchers.Main) {
                                errorMessage = "Tipo de cliente inválido. Escriba 'físico' o 'jurídico'"
                            }
                            return@launch
                        }
                    }

                    val url = URL("http://10.0.2.2:5055/services/client/login/new")
                    val connection = url.openConnection() as HttpURLConnection
                    connection.requestMethod = "POST"
                    connection.setRequestProperty("Content-Type", "application/json")
                    connection.doOutput = true

                    val json = JSONObject().apply {
                        put("id", id.toIntOrNull() ?: 0) // lo convierte a int

                        put("name", name)
                        put("last_name1", lastName1)
                        put("last_name2", lastName2)
                        put("type", typeInt) //
                        put("username", username)
                        put("password", password)
                        put("monthly_income", income.toDoubleOrNull() ?: 0.0)
                        put("phone_number", phone)
                        put("address", address)
                        put("removed", 0)
                    }

                    val outputBytes = json.toString().toByteArray(Charsets.UTF_8)
                    Log.d("SIGNUP", "JSON enviado: $json")

                    connection.outputStream.write(outputBytes)

                    val responseCode = connection.responseCode
                    if (responseCode == 200 || responseCode == 201) {
                        withContext(Dispatchers.Main) {
                            navController.navigate("login")
                        }
                    } else {
                        withContext(Dispatchers.Main) {
                            errorMessage = "Error: Código $responseCode"
                        }
                    }
                } catch (e: Exception) {
                    withContext(Dispatchers.Main) {
                        errorMessage = "Error de red: ${e.message}"
                        Log.e("SIGNUP", "Error de red", e)
                    }
                }
            }
        }) {
            Text("Register")
        }

        if (errorMessage.isNotEmpty()) {
            Spacer(modifier = Modifier.height(16.dp))
            Text(errorMessage, color = MaterialTheme.colorScheme.error)
        }

        TextButton(onClick = { navController.popBackStack() }) {
            Text("Back to Login", textDecoration = TextDecoration.Underline)
        }
    }
}
