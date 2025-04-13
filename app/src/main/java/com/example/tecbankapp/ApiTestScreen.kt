package com.example.tecbankapp

import android.util.Log
import androidx.compose.foundation.layout.*
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Menu
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import kotlinx.coroutines.*
import java.io.BufferedReader
import java.io.InputStreamReader
import java.net.HttpURLConnection
import java.net.URL
import androidx.navigation.NavHostController

@Composable
fun ApiTestScreen(navController: NavHostController) {
    var response by remember { mutableStateOf("Esperando respuesta...") }

    Column(
        modifier = Modifier
            .fillMaxSize()
            .padding(24.dp),
        verticalArrangement = Arrangement.spacedBy(16.dp)
    ) {
        Button(onClick = {
            CoroutineScope(Dispatchers.IO).launch {
                try {
                    val url = URL("http://10.0.2.2:5041/api/clientes")
                    val connection = url.openConnection() as HttpURLConnection
                    connection.requestMethod = "GET"
                    connection.connectTimeout = 5000
                    connection.readTimeout = 5000

                    val code = connection.responseCode
                    if (code == HttpURLConnection.HTTP_OK) {
                        val input = BufferedReader(InputStreamReader(connection.inputStream))
                        val result = input.readText()
                        input.close()

                        response = result
                    } else {
                        response = "Error HTTP: $code"
                    }

                    connection.disconnect()
                } catch (e: Exception) {
                    response = "Error: ${e.message}"
                    Log.e("HTTP", "Exception", e)
                }
            }
        }) {
            Text("Consultar API")
        }

        Text("Respuesta del servidor:")
        Text(response)
    }
}

