<template>
  <div class="ma-5">
    <v-data-table
        :headers="headers"
        :items="desserts"
        sort-by="name"
        class="elevation-1"
    >
      <template v-slot:top>
        <v-toolbar
            flat
        >
          <v-toolbar-title><h4>Funcionários</h4></v-toolbar-title>
          <v-spacer></v-spacer>
          <v-dialog
              v-model="dialog"
              max-width="500px"
          >
            <template v-slot:activator="{ on, attrs }">
              <v-btn
                  color="primary"
                  dark
                  class="mb-2"
                  v-bind="attrs"
                  v-on="on"
              >
                Novo Funcionário
              </v-btn>
            </template>
            <v-card>
              <v-card-title>
                <span class="headline">{{ formTitle }}</span>
              </v-card-title>

              <v-card-text>
                <v-container>
                  <v-row>
                    <v-col
                        cols="12"
                        sm="6"
                        md="4"
                    >
                      <v-text-field
                          v-model="editedItem.name"
                          label="nome"
                      ></v-text-field>
                    </v-col>
                    <v-col
                        cols="12"
                        sm="6"
                        md="4"
                    >
                      <v-text-field
                          v-model="editedItem.cellphone"
                          label="Celular"
                      ></v-text-field>
                    </v-col>
                    <v-col
                        cols="12"
                        sm="6"
                        md="4"
                    >
                      <v-text-field
                          v-model="editedItem.email"
                          label="E-mail"
                      ></v-text-field>
                    </v-col>
                  </v-row>
                </v-container>
              </v-card-text>

              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn
                    color="blue darken-1"
                    text
                    @click="close"
                >
                  Cancelar
                </v-btn>
                <v-btn
                    color="blue darken-1"
                    text
                    @click="save"
                >
                  Salvar
                </v-btn>
              </v-card-actions>
            </v-card>
          </v-dialog>
          <v-dialog v-model="dialogDelete" max-width="500px">
            <v-card>
              <v-card-title class="headline">Tem certeza eu deseja excluir esse funcionário?</v-card-title>
              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="blue darken-1" text @click="closeDelete">Cancelar</v-btn>
                <v-btn color="blue darken-1" text @click="deleteItemConfirm">OK</v-btn>
                <v-spacer></v-spacer>
              </v-card-actions>
            </v-card>
          </v-dialog>
        </v-toolbar>
      </template>
      <template v-slot:item.actions="{ item }">
        <v-icon
            small
            class="mr-2"
            @click="editItem(item)"
        >
          mdi-pencil
        </v-icon>
        <v-icon
            small
            @click="deleteItem(item)"
        >
          mdi-delete
        </v-icon>
      </template>
      <template v-slot:no-data>
        <v-btn
            color="primary"
            @click="initialize"
        >
          Reset
        </v-btn>
      </template>
    </v-data-table>
  </div>
</template>

<script>
export default {
  name: 'Employee',
  data: () => ({
    dialog: false,
    dialogDelete: false,
    headers: [
      {text: 'Nome', value: 'name'},
      {text: 'Celular', value: 'cellphone'},
      {text: 'E-mail', value: 'email'},
      {text: 'Ações', value: 'actions', sortable: false},
    ],
    desserts: [],
    editedIndex: -1,
    editedItem: {
      name: '',
      cellphone: '',
      email: ''
    },
    defaultItem: {
      name: '',
      cellphone: '',
      email: ''
    },
  }),

  computed: {
    formTitle() {
      return this.editedIndex === -1 ? 'Inserir Funcionário' : 'Editar Funcionário'
    },
  },

  watch: {
    dialog(val) {
      val || this.close()
    },
    dialogDelete(val) {
      val || this.closeDelete()
    },
  },

  created() {
    this.initialize()
  },

  methods: {
    initialize() {
      this.desserts = [
        {
          name: 'Alberto dev1',
          cellphone: '(11) 99999-6235',
          email: 'Alberto.dev1@xpi.com.br',
        },
        {
          name: 'Bruno dev2',
          cellphone: '(11) 99999-8567',
          email: 'bruno.dev2@xpi.com.br',
        },
        {
          name: 'Camila techlead',
          cellphone: '(11) 99999-7586',
          email: 'Camila.techled@xpi.com.br',
        },
        {
          name: 'Danilo gerente',
          cellphone: '(11) 99999-6358',
          email: 'Danilo.gerente@xpi.com.br',
        },
        {
          name: 'Eduarda coordenadora',
          cellphone: '(11) 99999-2378',
          email: 'eduarda.coordenadora@xpi.com.br',
        },
        {
          name: 'Benchimol',
          cellphone: '(11) 99999-4586',
          email: 'benchimol@xpi.com.br',
        },
      ]
    },

    editItem(item) {
      this.editedIndex = this.desserts.indexOf(item)
      this.editedItem = Object.assign({}, item)
      this.dialog = true
    },

    deleteItem(item) {
      this.editedIndex = this.desserts.indexOf(item)
      this.editedItem = Object.assign({}, item)
      this.dialogDelete = true
    },

    deleteItemConfirm() {
      this.desserts.splice(this.editedIndex, 1)
      this.closeDelete()
    },

    close() {
      this.dialog = false
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem)
        this.editedIndex = -1
      })
    },

    closeDelete() {
      this.dialogDelete = false
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem)
        this.editedIndex = -1
      })
    },

    save() {
      if (this.editedIndex > -1) {
        Object.assign(this.desserts[this.editedIndex], this.editedItem)
      } else {
        this.desserts.push(this.editedItem)
      }
      this.close()
    },
  },
}
</script>