@file:OptIn(ExperimentalMaterial3Api::class)

package com.example.tecbankapp

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
import com.example.tecbankapp.models.User
import androidx.navigation.NavController
import kotlinx.coroutines.launch


@Composable
fun HomeScreen(navController: NavController, user: User) {
    val drawerState = rememberDrawerState(initialValue = DrawerValue.Closed)
    val scope = rememberCoroutineScope()

    // Drawer a la izquierda con menú
    ModalNavigationDrawer(
        drawerState = drawerState,
        drawerContent = {
            ModalDrawerSheet {
                Text(
                    text = "Menú",
                    modifier = Modifier.padding(16.dp),
                    style = MaterialTheme.typography.titleLarge
                )

                NavigationDrawerItem(
                    label = { Text("Cuentas") },
                    selected = false,
                    onClick = {
                        scope.launch { drawerState.close() }
                        navController.navigate("cuentas")
                    }
                )

                NavigationDrawerItem(
                    label = { Text("Tarjetas") },
                    selected = false,
                    onClick = {
                        scope.launch { drawerState.close() }
                        navController.navigate("tarjetas")
                    }
                )

                NavigationDrawerItem(
                    label = { Text("Préstamos") },
                    selected = false,
                    onClick = {
                        scope.launch { drawerState.close() }
                        navController.navigate("prestamos")
                    }
                )
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
                            Icon(Icons.Default.Menu, contentDescription = "Menú")
                        }
                    }
                )
            }
        ) { padding ->
            Column(
                modifier = Modifier
                    .fillMaxSize()
                    .padding(padding)
                    .padding(32.dp)
                    .verticalScroll(rememberScrollState()),
                verticalArrangement = Arrangement.Center,
                horizontalAlignment = Alignment.CenterHorizontally
            ) {
                Text("Welcome, ${user.username}", style = MaterialTheme.typography.headlineSmall)
                Spacer(modifier = Modifier.height(45.dp))
                Text("Address: ${user.address}", style = MaterialTheme.typography.bodyLarge)
                Spacer(modifier = Modifier.height(45.dp))
                Text("Phone number: ${user.phone}", style = MaterialTheme.typography.bodyLarge)
            }
        }
    }
}