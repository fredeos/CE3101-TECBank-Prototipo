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
fun PaymentsScreen(clientId: Int) {
    var error by remember { mutableStateOf("") }
    var payments by remember { mutableStateOf(listOf<String>()) }

    LaunchedEffect(clientId) {
        withContext(Dispatchers.IO) {
            try {
                val url = URL("http://10.0.2.2:5055/services/client/$clientId/loans/payments")
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
                        val total = obj.getDouble("total")
                        val date = obj.getString("date").substringBefore("T")
                        val state = if (obj.getInt("state") == 0) "Pendiente" else "Pagado"
                        val type = if (obj.getInt("type") == 1) "Crédito" else "Otro"

                        list.add("ID: $id | Fecha: $date | Total: ₡$total | Estado: $state | Tipo: $type")
                    }

                    withContext(Dispatchers.Main) {
                        payments = list
                    }
                } else {
                    withContext(Dispatchers.Main) {
                        error = "Error al obtener pagos: Código $code"
                    }
                }
            } catch (e: Exception) {
                withContext(Dispatchers.Main) {
                    error = "Error de red: ${e.message ?: "sin mensaje"}"
                    Log.e("PAYMENTS", "Excepción al obtener pagos", e)
                }
            }
        }
    }

    // UI
    Column(
        modifier = Modifier
            .fillMaxSize()
            .padding(16.dp)
    ) {
        Text("Pagos del Préstamo", style = MaterialTheme.typography.titleLarge)
        Spacer(modifier = Modifier.height(16.dp))

        if (error.isNotEmpty()) {
            Text("Error: $error", color = MaterialTheme.colorScheme.error)
        } else {
            if (payments.isEmpty()) {
                Text("No hay pagos registrados.")
            } else {
                payments.forEach { payment ->
                    Text(payment, modifier = Modifier.padding(8.dp))
                    Divider()
                }
            }
        }
    }
}


