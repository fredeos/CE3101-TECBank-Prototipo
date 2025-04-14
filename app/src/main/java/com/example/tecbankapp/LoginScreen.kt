@file:OptIn(ExperimentalMaterial3Api::class)

package com.example.tecbankapp

import android.util.Log
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.*
import androidx.compose.ui.text.input.*
import androidx.compose.ui.unit.*
import androidx.navigation.NavHostController
import kotlinx.coroutines.*
import org.json.JSONObject
import java.io.*
import java.net.*
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextDecoration
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.Alignment




@Composable
fun LoginScreen(navController: NavHostController) {
    var username by remember { mutableStateOf("") }
    var password by remember { mutableStateOf("") }
    var errorMessage by remember { mutableStateOf("") }

    Column(
        modifier = Modifier
            .fillMaxSize()
            .padding(24.dp),
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.Center
    ) {
        Text("Welcome to TecBank", fontSize = 22.sp, fontWeight = FontWeight.Bold)

        Spacer(modifier = Modifier.height(24.dp))

        Text("User name:", fontWeight = FontWeight.Bold)

        OutlinedTextField(
            value = username,
            onValueChange = { username = it },
            modifier = Modifier.fillMaxWidth()
        )

        Spacer(modifier = Modifier.height(16.dp))

        Text("Password:", fontWeight = FontWeight.Bold)
        OutlinedTextField(
            value = password,
            onValueChange = { password = it },
            modifier = Modifier.fillMaxWidth(),
            visualTransformation = PasswordVisualTransformation(),
            keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Password)
        )

        Spacer(modifier = Modifier.height(24.dp))

        Button(onClick = {
            CoroutineScope(Dispatchers.IO).launch {
                try {
                    val url = URL("http://10.0.2.2:5055/services/client/login?user=$username&pass=$password")
                    val connection = url.openConnection() as HttpURLConnection
                    connection.requestMethod = "GET"
                    connection.connectTimeout = 5000
                    connection.readTimeout = 5000
                    connection.setRequestProperty("Content-Type", "application/json")

                    val json = JSONObject()
                    json.put("user", username)
                    json.put("pass", password)



                    //Lee la respuesta
                    val code = connection.responseCode
                    Log.d("LOGIN", "Código HTTP recibido: $code")
                    val response: String

                    if (code == 200) {
                        val input = BufferedReader(InputStreamReader(connection.inputStream))
                        response = input.readText()
                        input.close()

                        val jsonResponse = JSONObject(response)
                        val clientId = jsonResponse.getInt("id")
                        //val accountId = jsonResponse.getString("accountId")
                        // Extraer los campos del nombre
                        val name = jsonResponse.getString("name")
                        val last1 = jsonResponse.getString("last_name1")
                        val last2 = jsonResponse.getString("last_name2")

                        val fullName = "$name $last1 $last2"

                        // Si todo sale bien, va a otra pantalla
                        withContext(Dispatchers.Main) {
                            navController.navigate("home/${URLEncoder.encode(fullName, "UTF-8")}/$clientId")


                            Log.d("LOGIN", "Login exitoso. Navegando a Home")

                        }
                    } else {
                        val error = BufferedReader(InputStreamReader(connection.errorStream))
                        response = error.readText()
                        error.close()

                        withContext(Dispatchers.Main) {
                            Log.e("LOGIN", "Error HTTP: $code\n$response")
                            Log.e("LOGIN", errorMessage)
                        }
                    }

                } catch (e: Exception) {
                    withContext(Dispatchers.Main) {
                        errorMessage = "Network error: ${e.message}"
                        Log.e("LOGIN", errorMessage)
                    }
                }
            }
        }) {
            Text("Login")
        }

        if (errorMessage.isNotEmpty()) {
            Spacer(modifier = Modifier.height(16.dp))
            Text(errorMessage, color = MaterialTheme.colorScheme.error)
        }

        Spacer(modifier = Modifier.height(16.dp))
        Text("Don't you have an account?")
        TextButton(onClick = { /* Aún no implementado */ }) {
            Text("Sign up", textDecoration = TextDecoration.Underline)
        }
    }
}
