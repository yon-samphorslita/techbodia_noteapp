<template>
  <div class="flex h-screen w-[100vw] bg-[#e5ffe9] font-sans relative">
    <!-- NoteList handles its own search/filter/sort -->
    <NoteList
      :notes="notes"
      :selectedNoteId="selectedNote?.id || null"
      @selectNote="selectedNote = $event"
      @editNote="startEdit"
      @deleteNote="deleteNote"
    />

    <main class="flex-1 p-10 overflow-y-auto bg-[#e5ffe9]">
      <NoteForm
        v-if="showCreateForm || editingNote"
        :mode="editingNote ? 'edit' : 'create'"
        :note="editingNote"
        @submit="handleFormSubmit"
        @cancel="cancelForm"
      />
      <NoteDetail v-else-if="selectedNote" :note="selectedNote" />
      <div v-else class="text-center mx-auto">
        <h1 class="text-3xl font-bold mt-6 text-green-600">
          Write down your ideas
        </h1>
        <p class="text-gray-600 mt-2 text-lg">
          Start capturing your thoughts and inspirations.
        </p>
      </div>
    </main>

    <button
      @click="toggleCreateForm"
      class="fixed bottom-6 right-6 bg-green-600 hover:bg-green-600 text-white w-14 h-14 rounded-full shadow-lg text-2xl flex items-center justify-center"
    >
      <span v-if="!showCreateForm && !editingNote">+</span>
      <span v-else>&times;</span>
    </button>
  </div>
</template>
<script setup lang="ts">
import { ref, onMounted } from "vue";
import axios from "axios";
import NoteList from "../components/NoteList.vue";
import NoteDetail from "../components/NoteDetails.vue";
import NoteForm from "../components/NoteForm.vue";
import type { Note } from "../types/Note";

const notes = ref<Note[]>([]);
const selectedNote = ref<Note | null>(null);
const editingNote = ref<Note | null>(null);
const showCreateForm = ref(false);

const fetchNotes = async () => {
  const res = await axios.get("http://localhost:5039/api/Note");
  notes.value = res.data;
};

const handleFormSubmit = async ({
  title,
  content,
}: {
  title: string;
  content: string;
}) => {
  if (editingNote.value) {
    await axios.put(`http://localhost:5039/api/Note/${editingNote.value.id}`, {
      ...editingNote.value,
      title,
      content,
    });
  } else {
    await axios.post("http://localhost:5039/api/Note", { title, content });
  }

  await fetchNotes();
  cancelForm();
};

const deleteNote = async (id: number) => {
  if (confirm("Are you sure?")) {
    await axios.delete(`http://localhost:5039/api/Note/${id}`);
    await fetchNotes();
    if (selectedNote.value?.id === id) selectedNote.value = null;
  }
};

const startEdit = (note: Note) => {
  editingNote.value = note;
  selectedNote.value = null;
  showCreateForm.value = false;
};

const cancelForm = () => {
  editingNote.value = null;
  showCreateForm.value = false;
};

const toggleCreateForm = () => {
  if (editingNote.value) {
    editingNote.value = null;
  } else {
    showCreateForm.value = !showCreateForm.value;
    selectedNote.value = null;
  }
};

onMounted(fetchNotes);
</script>
