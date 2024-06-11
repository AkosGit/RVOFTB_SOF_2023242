import axios from 'axios'
import { BaseClient } from './BaseClient'
import type { CollectionModel } from '@/models/CollectionModel'
import type { SnippetModel } from '@/models/SnippetModel'

export class CollectionClient extends BaseClient {
  public async GetCollections(): Promise<Array<CollectionModel>> {
    try {
      const resp = await this.GetAxios().get(this.baseUrl + '/collection/', {})
      return resp.data
    } catch (error: any) {
      if (error.response) {
        throw new Error(`Failed to fetch data: "${error.response.data}"`)
      }
      throw new Error(`Failed to fetch data: ${error}`)
    }
  }

  public async GetSnippetsFromCollection(collectionID: string): Promise<Array<SnippetModel>> {
    try {
      const resp = await this.GetAxios().get(
        this.baseUrl + '/collection/GetSnippetsFromCollection/' + collectionID,
        {}
      )
      return resp.data
    } catch (error: any) {
      if (error.response) {
        throw new Error(`Failed to fetch data: "${error.response.data}"`)
      }
      throw new Error(`Failed to fetch data: ${error}`)
    }
  }
  public async GetSnippetsWithoutCollection(): Promise<Array<SnippetModel>> {
    try {
      const resp = await this.GetAxios().get(
        this.baseUrl + '/collection/GetSnippetsWithoutCollection/',
        {}
      )
      return resp.data
    } catch (error: any) {
      if (error.response) {
        throw new Error(`Failed to fetch data: "${error.response.data}"`)
      }
      throw new Error(`Failed to fetch data: ${error}`)
    }
  }
  public async AddNewCollection(collectionName: string): Promise<Array<SnippetModel>> {
    try {
      const resp = await this.GetAxios().post(this.baseUrl + '/collection/', {
        CollectionID: '',
        CollectionName: collectionName
      })
      return resp.data
    } catch (error: any) {
      if (error.response) {
        throw new Error(`Failed to fetch data: "${error.response.data}"`)
      }
      throw new Error(`Failed to fetch data: ${error}`)
    }
  }
  public async RemoveCollection(collectionID: string): Promise<string> {
    try {
      const resp = await this.GetAxios().delete(this.baseUrl + '/collection/' + collectionID, {})
      return resp.data
    } catch (error: any) {
      if (error.response) {
        throw new Error(`Failed to fetch data: "${error.response.data}"`)
      }
      throw new Error(`Failed to fetch data: ${error}`)
    }
  }
  public async AddSnippetToCollection(
    collectionName: string,
    snippetID: string
  ): Promise<Array<SnippetModel>> {
    try {
      const resp = await this.GetAxios().post(this.baseUrl + '/collection/AddSnippetToCollection', {
        CollectionName: collectionName,
        SnippetID: snippetID
      })
      return resp.data
    } catch (error: any) {
      if (error.response) {
        throw new Error(`Failed to fetch data: "${error.response.data}"`)
      }
      throw new Error(`Failed to fetch data: ${error}`)
    }
  }
  public async GetCollectionById(CollectionID: string): Promise<CollectionModel> {
    try {
      const resp = await this.GetAxios().get(this.baseUrl + '/collection/' + CollectionID, {})
      return resp.data
    } catch (error: any) {
      if (error.response) {
        throw new Error(`Failed to fetch data: "${error.response.data}"`)
      }
      throw new Error(`Failed to fetch data: ${error}`)
    }
  }
  public async RemoveSnippetFromCollection(
    collectionName: string,
    snippetID: string
  ): Promise<Array<SnippetModel>> {
    try {
      const resp = await this.GetAxios().post(
        this.baseUrl + '/collection/RemoveSnippetFromCollection',
        {
          CollectionName: collectionName,
          SnippetID: snippetID
        }
      )
      return resp.data
    } catch (error: any) {
      if (error.response) {
        throw new Error(`Failed to fetch data: "${error.response.data}"`)
      }
      throw new Error(`Failed to fetch data: ${error}`)
    }
  }
  public async RemoveSnippetFromCollectionById(
    CollectionID: string,
    snippetID: string
  ): Promise<Array<SnippetModel>> {
    try {
      //TODO: make this delete
      const resp = await this.GetAxios().post(
        this.baseUrl + '/collection/RemoveSnippetFromCollectionByCollectionID',
        {
          CollectionID: CollectionID,
          SnippetID: snippetID
        }
      )
      return resp.data
    } catch (error: any) {
      if (error.response) {
        throw new Error(`Failed to fetch data: "${error.response.data}"`)
      }
      throw new Error(`Failed to fetch data: ${error}`)
    }
  }
}
