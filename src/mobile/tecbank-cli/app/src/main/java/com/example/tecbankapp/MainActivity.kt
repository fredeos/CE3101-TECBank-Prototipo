@file:OptIn(ExperimentalMaterial3Api::class)

package com.example.tecbankapp

import com.example.tecbankapp.RegisterScreen
import com.example.tecbankapp.ApiTestScreen
import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.material3.*
import androidx.compose.runtime.Composable
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import com.example.tecbankapp.ui.theme.Screen
import androidx.navigation.NavHostController
import androidx.compose.foundation.layout.*
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.Alignment
import androidx.compose.ui.unit.dp
import androidx.compose.ui.text.font.FontWeight
import com.example.tecbankapp.models.User


class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            MyApp()
        }
    }
}



@Composable
fun MyApp() {
    val navController = rememberNavController()

    MaterialTheme {
        NavHost(navController = navController, startDestination = Screen.Login.route) {
            composable(Screen.Login.route) {
                LoginScreen(
                    navController = navController,
                    onLoginSuccess = {
                        navController.navigate(Screen.Api.route)
                    },
                    onGoToRegister = {
                        navController.navigate(Screen.Register.route)
                    }
                )
            }
            composable(Screen.Api.route) {
                //ApiTestScreen(navController = navController)

                // Simulamos un usuario por ahora (esto normalmente vendría del login)
                val user = remember {
                    User(
                        username = "Jessica",
                        address = "Cartago",
                        phone = "82828282"
                    )
                }
                HomeScreen(navController = navController, user = user)
            }

// Agregamos rutas en blanco
            composable("cuentas") { BlankScreen("Cuentas") }
            composable("tarjetas") { BlankScreen("Tarjetas") }
            composable("prestamos") { BlankScreen("Préstamos") }

            composable(Screen.Register.route) {
                RegisterScreen(
                    navController = navController,
                    onRegisterSuccess = {
                        navController.navigate(Screen.Login.route)
                    }
                )
            }
        }
    }
}

@Composable
fun BlankScreen(title: String) {
    Box(
        modifier = Modifier.fillMaxSize(),
        contentAlignment = Alignment.Center
    ) {
        Text("Pantalla de $title (próximamente)", style = MaterialTheme.typography.titleMedium)
    }
}
