<template>
  <div>
    <h1 class="ml-2 mb-3 pb-1">
      Organisatie Profiel<span v-if="organisatie">: {{ organisatie.name }}</span>
    </h1>
    <div class="d-flex">
      <form 
        class="mr-4"
        style="width: 570px"
        @submit.prevent="handleUpdateOrg">

        <div class="panel px-4 py-3 mb-2">
          <h2 class="font-weight-bold mt-1 mb-4">Algemeen</h2>
          
          <Feedback :feedback="feedback" />

          <ProfileSetting 
            label="Naam"
            :editMode="editMode"
            :disabled="true"
            v-model="organisatie.name" />
          <ProfileSetting 
            label="Email" 
            :editMode="editMode" 
            v-model="organisatie.email" />
          <ProfileSetting 
            label="Telefoonnummer" 
            :editMode="editMode"
            v-model="organisatie.phone_number" />
        </div>
        <div class="panel px-4 py-3 mb-2">
          <h2 class="font-weight-bold mt-1 mb-4">Factuur informatie</h2>
          <ProfileSetting 
            label="Naam" 
            :editMode="editMode"
            v-model="organisatie.invoice_name" />
          <ProfileSetting 
            label="PO nummer" 
            :editMode="editMode"
            v-model="organisatie.invoice_po_number" />
          <ProfileSetting 
            label="E-mail adres" 
            :editMode="editMode"
            v-model="organisatie.invoice_email" />
        </div>

        <div class="panel px-4 py-3 mb-2">
          <h2 class="font-weight-bold mt-1 mb-4">Bezoek adres</h2>
          <ProfileSetting 
            label="Straat" 
            :editMode="editMode"
            v-model="organisatie.home_street" />
          <ProfileSetting 
            label="Huisnummer" 
            :editMode="editMode"
            v-model="organisatie.home_address_number" />
          <ProfileSetting 
            label="Toevoeging" 
            :editMode="editMode"
            v-model="organisatie.home_address_number_postfix" />
          <ProfileSetting 
            label="Postbus" 
            :editMode="editMode"
            v-model="organisatie.home_postbox" />
          <ProfileSetting 
            label="Stad" 
            :editMode="editMode"
            v-model="organisatie.home_city" />
          <ProfileSetting 
            label="Postcode" 
            :editMode="editMode"
            v-model="organisatie.home_zipcode" />
          <ProfileSetting 
            label="Provincie" 
            :editMode="editMode"
            v-model="organisatie.home_state" />
          <ProfileSetting 
            label="Land" 
            :editMode="editMode"
            v-model="organisatie.home_country" />
        </div>
        <div class="panel px-4 py-3 mb-2">
          <h2 class="font-weight-bold mt-1 mb-4">Post adres</h2>
          <ProfileSetting 
            label="Straat" 
            :editMode="editMode"
            v-model="organisatie.postal_street" />
          <ProfileSetting 
            label="Huisnummer" 
            :editMode="editMode"
            v-model="organisatie.postal_address_number" />
          <ProfileSetting 
            label="Toevoeging" 
            :editMode="editMode"
            v-model="organisatie.postal_address_number_postfix" />
          <ProfileSetting 
            label="Postbus" 
            :editMode="editMode"
            v-model="organisatie.postal_postbox" />
          <ProfileSetting 
            label="Stad" 
            :editMode="editMode"
            v-model="organisatie.postal_city" />
          <ProfileSetting 
            label="Postcode" 
            :editMode="editMode"
            v-model="organisatie.postal_zipcode" />
          <ProfileSetting 
            label="Provincie" 
            :editMode="editMode"
            v-model="organisatie.postal_state" />
          <ProfileSetting 
            label="Land" 
            :editMode="editMode"
            v-model="organisatie.postal_country" />
        </div>

        <b-button 
          type="submit" 
          variant="primary" 
          class="SubmitButton font-weight-bold mt-4" 
          size="lg" 
          pill>
          <span class="d-inline-block my-2">
            Bewaar instellingen
          </span>
        </b-button>
        
      </form>

      <TeamMembersPanel />
    </div>

  </div>
</template>

<script>
import ProfileSetting from 'molecule/ProfileSetting'
import TeamMembersPanel from 'organism/TeamMembersPanel'
import Feedback from 'atom/Feedback'

import { image } from 'helper/assets'
import { isAdmin } from 'service/auth'

import { mapGetters, mapActions } from 'vuex'

export default {
  name: 'OrganisatieProfiel',
  
  components: {
    ProfileSetting,
    TeamMembersPanel,
    Feedback
  },
  data() {
    return {
      editMode: true,
      feedback: {}
    }
  },
  computed: {
    ...mapGetters('org', [
      'organization',
      'getOrgById'
    ]),
    organisatie() {
      return isAdmin() 
        ? this.getOrgById({ id : this.$route.params.id })
        : this.organization
    }
  },
  methods: {
    image,
    ...mapActions('org', [
      'updateOrg'
    ]),
    async handleUpdateOrg() {
      try {
        this.feedback = {
          variant: 'info', 
          message: 'Bezig met opslaan...'
        }
        await this.updateOrg({
          orgId: this.organisatie.getId(),
          data: this.organisatie
        });
        this.feedback = {
          variant: 'success', 
          message: 'Wijzigingen zijn opgeslagen'
        }
      } catch(err) {
        this.feedback = {
          variant: 'danger', 
          message: 'Wijzigingen zijn niet opgeslagen'
        }
      }
    }
  }
}
</script>

<style scoped lang="scss">

h1 {
  font-size: 30px;
  color: #354052;
}

.panel {
  background: white;
  border-radius: 4px;
  border: 1px solid #E6EAEE;

  h2 {
    font-size: 22px;
    color: #354052;
  }
}

</style>
<style lang="scss">

// TODO: Rename?
.SubmitButton {
  font-size: 18px;
  line-height: 1;
}

</style>
