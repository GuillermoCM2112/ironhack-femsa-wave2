#### CODIGO ANTERIOR
```
TEST UIResponsiveness
  UI_COMPONENT uiComponent = setupUIComponent(1024)
  ASSERT_TRUE(uiComponent.adjustsToScreenSize(1024), "UI should adjust to width of 1024 pixels")
END TEST


```

#### CODIGO REFACTORIZADO
```
// Función auxiliar para configurar el componente de UI para un tamaño específico
FUNCTION createUIComponentWithSize(screenWidth)
    UI_COMPONENT uiComponent = setupUIComponent(screenWidth)
    RETURN uiComponent
END FUNCTION

// Prueba para verificar el ajuste de UI en una pantalla de 1024 píxeles de ancho
TEST UIResponsiveness_AdjustTo1024Width
    // Crear el componente de UI con el ancho deseado usando la función auxiliar
    UI_COMPONENT uiComponent = createUIComponentWithSize(1024)

    // Verificar que el componente de UI se ajuste correctamente
    ASSERT_TRUE(uiComponent.adjustsToScreenSize(1024), "UI should adjust to width of 1024 pixels")
END TEST

// Prueba para verificar el ajuste de UI en una pantalla de 768 píxeles de ancho
TEST UIResponsiveness_AdjustTo768Width
    // Crear el componente de UI con el ancho deseado usando la función auxiliar
    UI_COMPONENT uiComponent = createUIComponentWithSize(768)

    // Verificar que el componente de UI se ajuste correctamente
    ASSERT_TRUE(uiComponent.adjustsToScreenSize(768), "UI should adjust to width of 768 pixels")
END TEST

// Prueba para verificar el ajuste de UI en una pantalla de 1440 píxeles de ancho
TEST UIResponsiveness_AdjustTo1440Width
    // Crear el componente de UI con el ancho deseado usando la función auxiliar
    UI_COMPONENT uiComponent = createUIComponentWithSize(1440)

    // Verificar que el componente de UI se ajuste correctamente
    ASSERT_TRUE(uiComponent.adjustsToScreenSize(1440), "UI should adjust to width of 1440 pixels")
END TEST

// Prueba para verificar el ajuste de UI en una pantalla de 320 píxeles de ancho
TEST UIResponsiveness_AdjustTo320Width
    // Crear el componente de UI con el ancho deseado usando la función auxiliar
    UI_COMPONENT uiComponent = createUIComponentWithSize(320)

    // Verificar que el componente de UI se ajuste correctamente
    ASSERT_TRUE(uiComponent.adjustsToScreenSize(320), "UI should adjust to width of 320 pixels")
END TEST

```
