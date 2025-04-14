// network/ApiService.ktpackage com.example.tecbankapp.network
package com.example.tecbankapp.network
import com.example.tecbankapp.models.Cliente
import retrofit2.Response
import retrofit2.http.*

interface ApiService {

    @POST("services/client/login/new")
    suspend fun registrarCliente(@Body cliente: Cliente): Response<Cliente>

    @GET("services/client/login")
    suspend fun obtenerClientePorUsuario(
        @Query("usuario") usuario: String,
        @Query("password") password: String
    ): Response<Cliente>
}