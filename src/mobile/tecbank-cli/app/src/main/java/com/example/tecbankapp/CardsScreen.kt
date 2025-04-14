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
fun CardsScreen(clientId: Int) {
    var cards by remember { mutableStateOf(listOf<String>()) }
    var error by remember { mutableStateOf("") }

    LaunchedEffect(clientId) {
        withContext(Dispatchers.IO) {
            try {
                val url = URL("http://10.0.2.2:5055/services/client/$clientId/cards")
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
                    val cardList = mutableListOf<String>()

                    for (i in 0 until jsonArray.length()) {
                        val card = jsonArray.getJSONObject(i)
                        val num = card.getInt("card_num")
                        val type = card.getInt("type")
                        val balance = card.getDouble("balance")
                        cardList.add("Tarjeta: $num | Tipo: $type | Saldo: ₡$balance")
                    }

                    withContext(Dispatchers.Main) {
                        cards = cardList
                    }
                } else {
                    withContext(Dispatchers.Main) {
                        error = "Error: Código $code"
                    }
                }
            } catch (e: Exception) {
                withContext(Dispatchers.Main) {
                    error = "Error de red: ${e.message}"
                    Log.e("CARDS", "Excepción al obtener tarjetas", e)
                }
            }
        }
    }

    Column(
        modifier = Modifier
            .fillMaxSize()
            .padding(16.dp)
    ) {
        Text("Tarjetas del Cliente", style = MaterialTheme.typography.titleLarge)
        Spacer(modifier = Modifier.height(16.dp))

        if (error.isNotEmpty()) {
            Text("Error: $error", color = MaterialTheme.colorScheme.error)
        } else {
            if (cards.isEmpty()) {
                Text("Este cliente no tiene tarjetas registradas.")
            } else {
                cards.forEach { card ->
                    Text(text = card, modifier = Modifier.padding(8.dp))
                    Divider()
                }
            }
        }
    }
}

