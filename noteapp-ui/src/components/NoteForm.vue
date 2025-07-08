<template>
  <div class="max-w-xl mx-auto bg-white pr-12 p-6 rounded-xl shadow-md">
    <h2 class="text-2xl font-bold mb-4 text-green-600">
      {{ mode === "edit" ? "Edit Note" : "Create a New Note" }}
    </h2>

    <form @submit.prevent="submit" class="space-y-4">
      <div>
        <label class="block text-left text-sm font-medium text-gray-700 mb-1"
          >Title *</label
        >
        <input
          v-model="form.title"
          type="text"
          required
          class="w-full p-3 border text-black border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-green-400 bg-white"
        />
      </div>

      <div>
        <label class="block text-left text-sm font-medium text-gray-700 mb-1"
          >Content</label
        >
        <textarea
          v-model="form.content"
          rows="5"
          class="w-full p-3 border text-black border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-green-400 bg-white"
        ></textarea>
      </div>

      <div class="flex gap-3">
        <button
          type="submit"
          class="bg-green-500 text-white px-4 py-2 rounded-lg hover:bg-green-600"
        >
          {{ mode === "edit" ? "Save" : "Add Note" }}
        </button>
        <button
          @click="$emit('cancel')"
          type="button"
          class="text-white hover:underline bg-red-700"
        >
          Cancel
        </button>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from "vue";
import type { Note } from "../types/Note";

const props = defineProps<{
  mode: "create" | "edit";
  note?: Note | null;
}>();

const emit = defineEmits<{
  (e: "submit", note: { title: string; content: string }): void;
  (e: "cancel"): void;
}>();

const form = ref({ title: "", content: "" });

watch(
  () => props.note,
  () => {
    form.value.title = props.note?.title || "";
    form.value.content = props.note?.content || "";
  },
  { immediate: true },
);

const submit = () => emit("submit", { ...form.value });
</script>
