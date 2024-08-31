<template>
  <v-card>
    <v-toolbar dark color="primary">
      <v-btn icon dark @click="onClose">
        <v-icon>fa-times-circle</v-icon>
      </v-btn>
      <v-toolbar-title>Student</v-toolbar-title>
      <v-spacer></v-spacer>
      <v-toolbar-items>
        <v-btn dark text @click="onSave" :disabled="!valid" :loading="loading">
          <v-icon class="px-2">far fa-save</v-icon> Save
        </v-btn>
      </v-toolbar-items>
    </v-toolbar>

    <v-card-text>
      <v-container>
        <v-form v-model="valid">
          <v-row>
            <v-col cols="12" sm="12">
              <v-text-field
                v-model="student.Nome"
                :rules="nomeRule"
                label="Nome do Estudante *"
                append-icon="fa-newspaper"
                placeholder="Nome completo do estudante.."
                outlined
                required
              ></v-text-field>
            </v-col>
            <v-col cols="12" sm="12">
              <v-text-field
                v-model="student.Email"
                :rules="emailRule"
                label="E-mail"
                append-icon="fa-file-alt"
                placeholder="Melhor e-mail do estudante..."
                outlined
              ></v-text-field>
            </v-col>
            <v-col cols="12" sm="12">
              <v-text-field
                v-model="student.CPF"
                :rules="cpfRule"
                label="CPF"
                append-icon="fa-file-alt"
                placeholder="CPF do estudante"
                outlined
              ></v-text-field>
            </v-col>
          </v-row>
        </v-form>
      </v-container>
    </v-card-text>
  </v-card>
</template>

<script>
import { validateCPF } from '@/helper/helper'

let regexEmail = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
export default {
  props: ['student', 'loading'],
  data: () => ({
    valid: false,
    nomeRule: [
      (v) => v.length > 9 || 'O nome precisa de no mínimo 10 caracteres',
    ],
    emailRule: [
      (v) => !!regexEmail.test(v) || 'Informe um email válido',
      (v) => v.length > 9 || 'O e-mail precisa de no mínimo 10 caracteres',
    ],
    cpfRule: [
      (v) => !!v || 'O cpf é obrigatório',
      (v) => !!validateCPF(v) || 'Informe um CPF válido',
    ],
  }),
  methods: {
    onSave() {
      this.$emit('onSave', this.student)
    },
    onClose() {
      this.$emit('onClose')
    },
  },
}
</script>
