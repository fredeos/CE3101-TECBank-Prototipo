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
fun LoansScreen(clientId: Int) {
    var loans by remember { mutableStateOf(listOf<String>()) }
    var error by remember { mutableStateOf("") }

    LaunchedEffect(clientId) {
        withContext(Dispatchers.IO) {
            try {
                val url = URL("http://10.0.2.2:5055/services/client/$clientId/loans")
                val connection = url.openConnection() as HttpURLConnection
                connection.requestMethod = "GET"
                connection.connectTimeout = 5000
                connection.readTimeout = 5000
                connection.setRequestProperty("Content-Type", "application/json")

                val code = connection.responseCode
                if (code == 200) {
                    val input = BufferedReader(InputStreamReader(connection.inputStream))
                    val response = input.readText()
                    input.close()

                    val jsonArray = JSONArray(response)
                    val loanList = mutableListOf<String>()

                    for (i in 0 until jsonArray.length()) {
                        val loan = jsonArray.getJSONObject(i)
                        val id = loan.getInt("id")
                        val total = loan.getDouble("total")
                        val state = loan.getInt("state")
                        val fecha = loan.getString("request_date").substringBefore("T")
                        loanList.add("Préstamo #$id | Total: ₡$total | Estado: $state | Fecha: $fecha")
                    }

                    withContext(Dispatchers.Main) {
                        loans = loanList
                    }
                } else {
                    withContext(Dispatchers.Main) {
                        error = "Error: Código $code"
                    }
                }
            } catch (e: Exception) {
                withContext(Dispatchers.Main) {
                    error = "Error de red: ${e.message}"
                    Log.e("LOANS", "Excepción al obtener préstamos", e)
                }
            }
        }
    }

    Column(
        modifier = Modifier
            .fillMaxSize()
            .padding(16.dp)
    ) {
        Text("Préstamos del Cliente", style = MaterialTheme.typography.titleLarge)
        Spacer(modifier = Modifier.height(16.dp))

        if (error.isNotEmpty()) {
            Text("Error: $error", color = MaterialTheme.colorScheme.error)
        } else {
            if (loans.isEmpty()) {
                Text("Este cliente no tiene préstamos.")
            } else {
                loans.forEach { loan ->
                    Text(text = loan, modifier = Modifier.padding(8.dp))
                    Divider()
                }
            }
        }
    }
}
