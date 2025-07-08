<template>
  <aside class="w-1/3 bg-white border-r border-green-300 p-6 overflow-y-auto">
    <h2 class="text-2xl font-bold mb-6 text-green-600">Your Notes</h2>

    <!-- Search + Sort -->
    <div class="flex flex-row gap-3 mb-6">
      <input
        v-model="searchQuery"
        type="search"
        placeholder="Search notes..."
        class="w-2/4 bg-white text-black border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-green-500"
      />

      <select
        v-model="sortField"
        class="w-1/4 border bg-white text-black border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-green-500"
      >
        <option value="createdAt">Created At</option>
        <option value="updatedAt">Updated At</option>
      </select>

      <select
        v-model="sortOrder"
        class="w-1/4 border bg-white text-black border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-green-500"
      >
        <option value="asc">Ascending</option>
        <option value="desc">Descending</option>
      </select>
    </div>

    <!-- Notes List -->
    <div v-if="filteredNotes.length === 0" class="text-gray-500 italic">
      No notes found.
    </div>

    <div
      v-for="note in filteredNotes"
      :key="note.id"
      class="bg-green-50 border-l-4 border-green-400 rounded-lg p-4 mb-4 shadow hover:shadow-lg transition group cursor-pointer"
      :class="{ 'ring-2 ring-green-500': selectedNoteId === note.id }"
      @click="$emit('selectNote', note)"
    >
      <h3 class="text-lg font-semibold text-green-700 flex justify-between">
        <span>{{ note.title }}</span>
        <span class="text-xs text-green-500 mt-1">
          {{ formatDate(note.createdAt) }}
        </span>
      </h3>
      <p class="text-gray-600 text-sm text-left">{{ note.content }}</p>

      <div
        class="flex justify-end gap-2 mt-2 hidden group-hover:flex transition-all duration-200"
      >
        <button
          @click.stop="$emit('editNote', note)"
          class="text-sm bg-white shadow text-blue-500"
        >
          Edit
        </button>
        <button
          @click.stop="$emit('deleteNote', note.id)"
          class="text-sm bg-white shadow text-red-500"
        >
          Delete
        </button>
      </div>
    </div>
  </aside>
</template>
<script setup lang="ts">
import { ref, computed } from "vue";
import type { Note } from "../types/Note";

const props = defineProps<{
  notes: Note[];
  selectedNoteId: number | null;
}>();

const searchQuery = ref("");
const sortField = ref<"createdAt" | "updatedAt">("createdAt");
const sortOrder = ref<"asc" | "desc">("asc");

const filteredNotes = computed(() => {
  let filtered = props.notes;

  // Search
  if (searchQuery.value.trim() !== "") {
    const term = searchQuery.value.toLowerCase();
    filtered = filtered.filter(
      (note) =>
        note.title?.toLowerCase().includes(term) ||
        note.content?.toLowerCase().includes(term),
    );
  }

  // Sort
  return filtered.slice().sort((a, b) => {
    const field = sortField.value;
    const aDate = new Date(a[field]);
    const bDate = new Date(b[field]);
    return sortOrder.value === "asc"
      ? aDate.getTime() - bDate.getTime()
      : bDate.getTime() - aDate.getTime();
  });
});

const formatDate = (dateStr: string) =>
  new Date(dateStr).toLocaleDateString(undefined, {
    year: "numeric",
    month: "short",
    day: "numeric",
  });
</script>
