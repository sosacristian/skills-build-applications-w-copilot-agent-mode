<template>
  <div class="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50 z-50">
    <div class="flex flex-col items-center justify-center space-y-4">
      <div class="animate-spin rounded-full h-16 w-16 border-t-4 border-b-4 border-primary-600"></div>
      <p v-if="message" class="text-white text-lg font-medium">{{ message }}</p>
    </div>
  </div>
</template>

<script setup lang="ts">
interface Props {
  message?: string
}

defineProps<Props>()
</script>

<style scoped>
@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}

.animate-spin {
  animation: spin 1s linear infinite;
}
</style>
