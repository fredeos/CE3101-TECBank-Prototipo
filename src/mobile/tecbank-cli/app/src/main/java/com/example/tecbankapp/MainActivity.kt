package com.example.tecbankapp

import android.os.Bundle
import android.util.Log
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.foundation.layout.*
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import java.io.BufferedReader
import java.io.InputStreamReader
import java.net.HttpURLConnection
import java.net.URL

class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            TecBankAppTheme {
                Surface(modifier = Modifier.fillMaxSize(), color = MaterialTheme.colorScheme.background) {
                    ApiTestScreen()
                }
            }
        }
    }
}

@Composable
fun ApiTestScreen() {
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
                    val str = 1
                    val url = URL("http://192.168.0.2:5055/services/admin/clients/${str}") // Cambia a tu IP si usás celular real
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

@Composable
fun TecBankAppTheme(content: @Composable () -> Unit) {
    MaterialTheme(
        colorScheme = lightColorScheme(), // tema claro
        content = content
    )
}
