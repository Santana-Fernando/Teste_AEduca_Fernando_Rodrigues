<template>
  <v-toolbar dark prominent :src="background">
    <v-toolbar-title class="ma-0 pa-0 text-h3">Matrículas</v-toolbar-title>

    <template v-slot:extension>
      <v-btn fab color="accent" bottom right absolute @click="OnAdd">
        <v-icon>fa-plus</v-icon>
      </v-btn>
    </template>

    <v-spacer></v-spacer>
  </v-toolbar>
</template>

<script>
export default {
  props: ['background'],
  methods: {
    OnAdd() {
      this.$emit('OnAdd')
    },
  },
}
</script>
