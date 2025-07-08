<template>
  <div class="max-w-xl mx-auto bg-white p-6 rounded-xl shadow-md">
    <h2 class="text-2xl font-bold mb-2 text-green-600">{{ note.title }}</h2>
    <p class="text-gray-700 mb-4 whitespace-pre-line">{{ note.content }}</p>
    <div class="text-sm text-gray-500 flex justify-between">
      <span>Created: {{ formatDate(note.createdAt) }}</span>
      <span>Updated: {{ formatDate(note.updatedAt) }}</span>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Note } from "../types/Note";

defineProps<{ note: Note }>();

const formatDate = (dateStr: string) =>
  new Date(dateStr).toLocaleDateString(undefined, {
    year: "numeric",
    month: "short",
    day: "numeric",
  });
</script>
