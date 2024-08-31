<template>
  <v-container>
    <v-dialog
      v-model="showDialog"
      fullscreen
      hide-overlay
      transition="dialog-bottom-transition"
    >
      <student-form
        :student="student"
        :loading="loading"
        @onSave="create"
        @onClose="$emit('onClose')"
      ></student-form>
    </v-dialog>
  </v-container>
</template>

<script>
import Api from '@/api/student.api.js'
import ApiResponseMixin from '@/mixins/ApiResponseMixin'
import StudentForm from '@/components/StudentForm'

export default {
  props: ['showDialog'],
  mixins: [ApiResponseMixin],
  data: () => ({
    loading: false,
    student: {
      RA: 0,
      Nome: '',
      Email: '',
      CPF: '',
    },
  }),
  components: {
    StudentForm,
  },
  methods: {
    clearFields() {
      this.student.Nome = ''
      this.student.Email = ''
      this.student.CPF = ''
    },
    create(student) {
      this.loading = true
      Api.create(student)
        .then(() => {
          this.$emit('onUpdated')
          this.clearFields()
        })
        .catch((error) => {
          console.log(error)
          this.$emit('onError', this.extractErrorFromResponse(error))
        })
        .finally(() => {
          this.loading = false
        })
    },
  },
}
</script>
