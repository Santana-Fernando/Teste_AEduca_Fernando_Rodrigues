<template>
  <v-main class="overflow-hidden">
    <app-bar
      :background="'https://cdn.vuetifyjs.com/images/backgrounds/vbanner.jpg'"
      @OnAdd="create"
    ></app-bar>

    <v-container>
      <v-row
        v-if="loading"
        class="fill-height"
        align-content="center"
        justify="center"
      >
        <v-col class="text-subtitle-1 text-center" cols="12">
          Getting list...
        </v-col>
        <v-col cols="6">
          <v-progress-linear
            color="blue accent-4"
            indeterminate
            rounded
            height="6"
          ></v-progress-linear>
        </v-col>
      </v-row>

      <v-row v-if="!loading">
        <v-col
          class="pa-10"
          cols="12"
          v-for="student in students"
          :key="student.RA"
        >
          <student-card
            :student="student"
            @onEdit="edit"
            @onRemove="remove"
          ></student-card>
        </v-col>
      </v-row>

      <v-row v-if="!loading" class="pa-5">
        <v-col cols="12">
          <div v-if="students && students.length == 0">
            <h2>Your list is empty 🥺</h2>
          </div>
          <div v-if="students && students.length > 3" class="pb-7">
            <h2>You've reached the end! 👋</h2>
          </div>
        </v-col>
      </v-row>

      <!-- Create Student Dialog -->
      <student-create-dialog
        :showDialog="showStudentCreateDialog"
        @onUpdated="studentCreated"
        @onClose="showStudentCreateDialog = false"
        @onError="showError"
      />

      <!-- Update Student Dialog -->
      <student-update-dialog
        :showDialog="showStudentUpdateDialog"
        :student="student"
        @onUpdated="studentSaved"
        @onClose="showStudentUpdateDialog = false"
        @onError="showError"
      />

      <!-- SNACKBAR TOAST -->
      <v-snackbar v-model="snackbar" :vertical="true" :top="true">
        {{ snackbar_text }}
      </v-snackbar>
    </v-container>
  </v-main>
</template>

<script>
import Api from '@/api/student.api.js'
import AppBar from '@/components/AppBar.vue'
import StudentCard from '@/components/StudentCard.vue'
import StudentCreateDialog from '@/components/StudentCreateDialog.vue'
import StudentUpdateDialog from '@/components/StudentUpdateDialog.vue'

export default {
  data: () => ({
    loading: false,
    students: [],
    showStudentCreateDialog: false,
    showStudentUpdateDialog: false,
    student: null,
    snackbar: false,
    snackbar_text: null,
  }),
  created() {
    this.loadStudents()
  },
  methods: {
    loadStudents() {
      this.loading = true
      console.log('Entrou na função')
      Api.list().then((response) => {
        console.log(response)
        this.students = response
        this.loading = false
      })

      console.log(this.students)
    },
    create() {
      this.showStudentCreateDialog = true
    },
    studentCreated() {
      this.loadStudents()
      this.showStudentCreateDialog = false
      this.snackbar_text = 'Student has been created!'
      this.snackbar = true
    },
    edit(student) {
      Api.get(student.RA).then((student) => {
        this.student = student
        this.showStudentUpdateDialog = true
      })
    },
    studentSaved() {
      this.loadStudents()
      this.showStudentUpdateDialog = false
      this.snackbar_text = 'Student saved successfuly!'
      this.snackbar = true
    },
    remove(student) {
      const confirmed = window.confirm(
        'Tem certeza de que deseja excluir este aluno?'
      )
      if (confirmed) {
        Api.delete(student.RA).then(() => {
          this.loadStudents()
        })
      }
    },
    showError(error) {
      this.snackbar_text = error
      this.snackbar = true
    },
  },
  components: { AppBar, StudentCard, StudentCreateDialog, StudentUpdateDialog },
}
</script>

<style lang="scss"></style>
