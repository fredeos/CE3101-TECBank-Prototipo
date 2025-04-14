@file:OptIn(ExperimentalMaterial3Api::class)

package com.example.tecbankapp

import android.util.Log
import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.material3.*
import androidx.compose.runtime.Composable
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import androidx.compose.foundation.layout.*
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.Alignment
import java.net.URLDecoder




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

    NavHost(navController = navController, startDestination = "login") {
        composable("login") {
            LoginScreen(navController = navController)
        }
        composable("home/{nombre}/{clientId}") { backStackEntry ->
            val username = backStackEntry.arguments?.getString("nombre") ?: "Usuario"
            val nombre = URLDecoder.decode(username, "UTF-8").replace("+", " ")
            val clientId = backStackEntry.arguments?.getString("clientId")?.toIntOrNull() ?: -1
            HomeScreen(navController = navController, username = nombre, clientId = clientId)
        }

        composable("accounts/{clientId}") { backStackEntry ->
            val clientId = backStackEntry.arguments?.getString("clientId")?.toIntOrNull() ?: -1
            if (clientId != -1) {
                AccountsScreen(clientId = clientId)
            } else {
                Text("Error: clientId inválido")
            }
        }

        composable("cards/{clientId}") { backStackEntry ->
            val clientId = backStackEntry.arguments?.getString("clientId")?.toIntOrNull() ?: -1
            CardsScreen(clientId)
        }

        composable("loans/{clientId}") { backStackEntry ->
            val clientId = backStackEntry.arguments?.getString("clientId")?.toIntOrNull() ?: -1
            LoansScreen(clientId)
        }

        composable("movements/{clientId}") { backStackEntry ->
            val clientId = backStackEntry.arguments?.getString("clientId")?.toIntOrNull() ?: -1
            MovementsScreen(clientId)
        }

        composable("payments/{clientId}") { backStackEntry ->
            val clientId = backStackEntry.arguments?.getString("clientId")?.toIntOrNull() ?: -1
            if (clientId != -1) {
                PaymentsScreen(clientId)
            } else {
                Text("Error: clientId inválido")
            }
        }



    }
}
