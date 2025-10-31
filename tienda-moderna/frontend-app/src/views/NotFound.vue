<template>
  <div class="max-w-2xl mx-auto text-center">
    <div class="card py-12">
      <div class="text-6xl mb-4">🔍</div>
      <h1 class="text-4xl font-bold mb-4">Página No Encontrada</h1>
      <p class="text-gray-600 mb-8">
        La página que buscas no existe o ha sido movida.
      </p>
      <RouterLink to="/" class="btn btn-primary">
        Volver al Inicio
      </RouterLink>
    </div>
  </div>
</template>

<script setup lang="ts">
import { RouterLink } from 'vue-router'
</script>
