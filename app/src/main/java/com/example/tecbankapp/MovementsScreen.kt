package com.example.tecbankapp


import androidx.compose.foundation.layout.*
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import org.json.JSONArray
import java.io.BufferedReader
import java.io.InputStreamReader
import java.net.HttpURLConnection
import java.net.URL
import android.util.Log
import kotlinx.coroutines.withContext
import kotlinx.coroutines.Dispatchers
@Composable
fun MovementsScreen(clientId: Int) {
    var error by remember { mutableStateOf("") }
    var movements by remember { mutableStateOf(listOf<String>()) }

    LaunchedEffect(clientId) {
        withContext(Dispatchers.IO) {
            try {
                // Paso 1: obtener cuentas
                val accountUrl = URL("http://10.0.2.2:5055/services/client/$clientId/accounts")
                val accountConnection = accountUrl.openConnection() as HttpURLConnection
                accountConnection.requestMethod = "GET"
                accountConnection.connectTimeout = 5000
                accountConnection.readTimeout = 5000
                accountConnection.setRequestProperty("Content-Type", "application/json")

                val accountCode = accountConnection.responseCode
                val accountsArray = if (accountCode == 200) {
                    val accountReader = BufferedReader(InputStreamReader(accountConnection.inputStream))
                    val accountResponse = accountReader.readText()
                    accountReader.close()
                    JSONArray(accountResponse)
                } else {
                    withContext(Dispatchers.Main) {
                        error = "Error al obtener cuentas: $accountCode"
                    }
                    return@withContext
                }

                if (accountsArray.length() == 0) {
                    withContext(Dispatchers.Main) {
                        error = "Este cliente no tiene cuentas"
                    }
                    return@withContext
                }

                val firstAccountId = accountsArray.getJSONObject(0).getString("id")

                // Paso 2: obtener movimientos de la primera cuenta
                val url = URL("http://10.0.2.2:5055/services/client/$clientId/$firstAccountId/movements")
                val connection = url.openConnection() as HttpURLConnection
                connection.requestMethod = "GET"
                connection.connectTimeout = 5000
                connection.readTimeout = 5000
                connection.setRequestProperty("Content-Type", "application/json")

                val code = connection.responseCode
                if (code == 200) {
                    val reader = BufferedReader(InputStreamReader(connection.inputStream))
                    val response = reader.readText()
                    reader.close()

                    val jsonArray = JSONArray(response)
                    val list = mutableListOf<String>()

                    for (i in 0 until jsonArray.length()) {
                        val obj = jsonArray.getJSONObject(i)
                        val id = obj.getString("id")
                        val desc = obj.getString("description")
                        val amount = obj.getDouble("total_transfer")
                        list.add("ID: $id | $desc | ₡$amount")
                    }

                    withContext(Dispatchers.Main) {
                        movements = list
                    }

                } else {
                    withContext(Dispatchers.Main) {
                        error = "Error al obtener movimientos: Código $code"
                    }
                }

            } catch (e: Exception) {
                withContext(Dispatchers.Main) {
                    error = "Error de red: ${e.message ?: "sin mensaje"}"
                    Log.e("MOVEMENTS", "Excepción al obtener movimientos", e)
                }
            }
        }
    }




    Column(modifier = Modifier.padding(16.dp)) {
        Text("Movimientos del Cliente", style = MaterialTheme.typography.titleLarge)
        Spacer(modifier = Modifier.height(16.dp))

        if (error.isNotEmpty()) {
            Text("Error: $error", color = MaterialTheme.colorScheme.error)
        } else if (movements.isEmpty()) {
            Text("No hay movimientos registrados.")
        } else {
            movements.forEach {
                Text(it, modifier = Modifier.padding(8.dp))
                Divider()
            }
        }
    }
}
