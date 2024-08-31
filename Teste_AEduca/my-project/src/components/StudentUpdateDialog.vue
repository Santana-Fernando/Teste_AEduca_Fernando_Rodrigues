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
        @onSave="update"
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
  props: ['showDialog', 'student'],
  data: () => ({
    loading: false,
  }),
  mixins: [ApiResponseMixin],
  components: {
    StudentForm,
  },
  methods: {
    update(student) {
      this.loading = true
      Api.update(student)
        .then(() => {
          this.$emit('onUpdated')
        })
        .catch((error) => {
          this.$emit('onError', this.extractErrorFromResponse(error))
        })
        .finally(() => {
          this.loading = false
        })
    },
  },
}
</script>
