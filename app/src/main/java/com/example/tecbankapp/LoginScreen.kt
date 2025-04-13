@file:OptIn(ExperimentalMaterial3Api::class)
package com.example.tecbankapp
import com.example.tecbankapp.ui.theme.Screen
import com.example.tecbankapp.network.RetrofitInstance
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import android.util.Log
import androidx.compose.foundation.layout.*
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.compose.ui.text.style.TextDecoration
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.sp
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.text.style.TextAlign
import androidx.navigation.NavHostController
import androidx.lifecycle.viewmodel.compose.viewModel

@Composable
fun LoginScreen(
    navController: NavHostController,
    onLoginSuccess: () -> Unit,
    onGoToRegister: () -> Unit
) {
    var username by remember { mutableStateOf("") }
    var password by remember { mutableStateOf("") }

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
            visualTransformation = PasswordVisualTransformation()
        )

        Spacer(modifier = Modifier.height(24.dp))

        Button(onClick = {
            CoroutineScope(Dispatchers.IO).launch {
                try {
                    val response = RetrofitInstance.api.obtenerClientePorUsuario(username, password)
                    if (response.isSuccessful) {
                        val cliente = response.body()
                        if (cliente != null) {
                            withContext(Dispatchers.Main) {
                                navController.navigate("api") {
                                    popUpTo(Screen.Login.route) { inclusive = true }
                                }
                            }
                        } else {
                            Log.e("LOGIN", "Usuario o contraseña incorrectos")
                        }
                    }
                } catch (e: Exception) {
                    Log.e("LOGIN", "Error: ${e.message}")
                }
            }
        }) {
            Text("Login")
        }


        Spacer(modifier = Modifier.height(16.dp))

        Text("Don't you have an account?")

        TextButton(onClick = onGoToRegister) {
            Text("Sign up", textDecoration = TextDecoration.Underline)
        }
    }
}