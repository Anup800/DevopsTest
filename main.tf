locals {
  common_tags = merge(
    {
      environment = var.environment
      managed_by  = "terraform"
      project     = "mytest-function-app"
    },
    var.tags
  )
}

resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = var.location
  tags     = local.common_tags
}

resource "azurerm_storage_account" "sa" {
  name                     = var.storage_account_name
  resource_group_name      = azurerm_resource_group.rg.name
  location                 = azurerm_resource_group.rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  tags                     = local.common_tags
}

resource "azurerm_service_plan" "plan" {
  name                = var.service_plan_name
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location

  os_type  = "Linux"
  sku_name = "Y1"

  tags = local.common_tags
}

resource "azurerm_linux_function_app" "func" {
  name                        = var.function_app_name
  resource_group_name         = azurerm_resource_group.rg.name
  location                    = azurerm_resource_group.rg.location
  service_plan_id             = azurerm_service_plan.plan.id
  https_only                  = true
  functions_extension_version = "~4"

  storage_account_name       = azurerm_storage_account.sa.name
  storage_account_access_key = azurerm_storage_account.sa.primary_access_key

  site_config {
    application_stack {
      dotnet_version               = "8.0"
      use_dotnet_isolated_runtime = true
    }
  }

  app_settings = {
    FUNCTIONS_WORKER_RUNTIME = "dotnet-isolated"
  }

  tags = local.common_tags
}

resource "azurerm_api_management" "apim" {
  name                = var.apim_name
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  publisher_name      = var.apim_publisher_name
  publisher_email     = var.apim_publisher_email

  sku_name = "Developer_1"

  identity {
    type = "SystemAssigned"
  }

  tags = local.common_tags
}
