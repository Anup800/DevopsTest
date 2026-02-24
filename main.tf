resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = var.location
}



resource "azurerm_service_plan" "plan" {
  name                = "func-linux-plan"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location

  os_type  = "Linux"
  sku_name = "Y1"   # Consumption plan
}

resource "azurerm_linux_function_app" "func" {
  name                       = var.function_app_name
  resource_group_name        = azurerm_resource_group.rg.name
  location                   = azurerm_resource_group.rg.location
  service_plan_id            = azurerm_service_plan.plan.id
  https_only                 = true
  functions_extension_version = "~4"

  storage_account_name       = azurerm_storage_account.sa.name
  storage_account_access_key = azurerm_storage_account.sa.primary_access_key


  site_config {
    application_stack {
      dotnet_version              = "8.0"
      use_dotnet_isolated_runtime = true
    }
  }

  app_settings = {
    FUNCTIONS_WORKER_RUNTIME = "dotnet-isolated"
  }
}

resource "azurerm_storage_account" "sa" {
  name                     = "stfuncbasic001"   # MUST be globally unique
  resource_group_name      = azurerm_resource_group.rg.name
  location                 = azurerm_resource_group.rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_api_management" "apim" {
  name                = "apim-demo-anup"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  publisher_name      = "Anup Singh"
  publisher_email     = "anup@example.com"

  sku_name = "Developer_1"

  identity {
    type = "SystemAssigned"
  }
}


