package com.example.tecbankapp.ui.theme

// Navigation.kt o Screen.kt

sealed class Screen(val route: String) {
    object Login : Screen("login")
    object Api : Screen("api")
    object Register : Screen("register")
}
