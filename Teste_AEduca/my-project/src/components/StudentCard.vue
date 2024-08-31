<template>
  <v-card>
    <v-card-text>
      <div class="mb-2"><strong>RA:</strong> {{ student.RA }}</div>
      <div class="mb-2">
        <strong class="text-subtitle-1 text--primary">Nome:</strong>
        {{ student.Nome }}
      </div>
      <div class="mb-2">
        <strong class="text-subtitle-1 text--primary">E-mail:</strong>
        {{ student.Email }}
      </div>
      <div>
        <strong class="text-subtitle-1 text--primary">CPF:</strong>
        {{ student.CPF }}
      </div>
    </v-card-text>
    <v-card-actions>
      <v-list-item class="grow">
        <v-row align="center" justify="end">
          <v-btn icon color="grey" @click="edit"
            ><v-icon>fas fa-pen</v-icon></v-btn
          >
          <v-btn icon color="grey" @click="remove"
            ><v-icon>far fa-trash-alt</v-icon></v-btn
          >
        </v-row>
      </v-list-item>
    </v-card-actions>
  </v-card>
</template>

<script>
export default {
  props: ['student'],
  data: () => ({
    updateLoading: false,
    removeLoading: false,
  }),
  methods: {
    edit() {
      this.$emit('onEdit', this.student)
    },
    remove() {
      this.$emit('onRemove', this.student)
    },
  },
}
</script>
