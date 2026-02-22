output "function_app_name" {
  value = azurerm_linux_function_app.func.name
}

output "function_app_url" {
  value = azurerm_linux_function_app.func.default_hostname
}
