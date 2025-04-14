
@file:OptIn(ExperimentalMaterial3Api::class)


package com.example.tecbankapp
import android.adservices.adid.AdId
import android.util.Log
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Menu
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.navigation.NavHostController
import kotlinx.coroutines.launch
import androidx.compose.ui.unit.sp
import kotlinx.coroutines.CoroutineScope



@Composable
fun HomeScreen(navController: NavHostController, username:String, clientId: Int) {
    val drawerState = rememberDrawerState(initialValue = DrawerValue.Closed)
    val scope = rememberCoroutineScope()

    ModalNavigationDrawer(
        drawerState = drawerState,
        drawerContent = {
            ModalDrawerSheet {
                Text(
                    "Menu",
                    style = MaterialTheme.typography.titleLarge,
                    modifier = Modifier.padding(16.dp)
                )

                // Verificar que clientId y accountId no estén vacíos
                if (clientId != -1) {
                    DrawerItem("Accounts", navController, "accounts/$clientId", drawerState, scope)
                } else {
                    Log.e("HomeScreen", "clientId no válido")
                }

                DrawerItem("Cards", navController, "cards/$clientId", drawerState, scope)
                DrawerItem("Loans", navController, "loans/$clientId", drawerState, scope)
                DrawerItem("Movements", navController, "movements/$clientId", drawerState, scope)
                DrawerItem("Payments", navController, "payments/$clientId", drawerState, scope)
            }
        }
    ) {
        Scaffold(
            topBar = {
                TopAppBar(
                    title = { Text("TecBank") },
                    navigationIcon = {
                        IconButton(onClick = {
                            scope.launch { drawerState.open() }
                        }) {
                            Icon(Icons.Default.Menu, contentDescription = "Menu")
                        }
                    }
                )
            }
        ) { padding ->
            Box(
                modifier = Modifier
                    .fillMaxSize()
                    .padding(padding),
                contentAlignment = Alignment.Center
            ) {
                Text("Bienvenido, $username", style = MaterialTheme.typography.headlineSmall)
            }
        }
    }
}


@Composable
fun DrawerItem(
    title: String,
    navController: NavHostController,
    route: String,
    drawerState: DrawerState,
    scope: CoroutineScope
) {
    NavigationDrawerItem(
        label = { Text(title) },
        selected = false,
        onClick = {
            scope.launch { drawerState.close() }
            navController.navigate(route)
        },
        modifier = Modifier.padding(NavigationDrawerItemDefaults.ItemPadding)
    )
}

@Composable
fun SimpleScreen(title: String) {
    Box(
        modifier = Modifier.fillMaxSize(),
        contentAlignment = Alignment.Center
    ) {
        Text("$title screen coming soon", style = MaterialTheme.typography.titleMedium)
    }
}


