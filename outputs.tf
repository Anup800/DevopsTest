output "environment" {
  value = var.environment
}

output "resource_group_name" {
  value = azurerm_resource_group.rg.name
}

output "function_app_name" {
  value = azurerm_linux_function_app.func.name
}

output "function_app_url" {
  value = azurerm_linux_function_app.func.default_hostname
}
